using System;
using System.Threading;
using System.Threading.Tasks;
using BackgroundService.NET.CronHostService;
using BackgroundService.NET.CronJob;
using BackgroundService.NET.CronJobProviderService;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using Shouldly;

namespace BackgroundService.NET.Tests;

[TestFixture]
public class CronHostTests
{
    [Test]
    public async Task ExecuteAsync_CronJobDoesNotThrow_ExecutedContinuously()
    {
        // Arrange
        var job = new TestJob(false);
        var sut = GetSut(job, "* * * * * *");
        var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.CancelAfter(TimeSpan.FromMilliseconds(100));

        // Act
        var task = sut.ExecuteAsync(cancellationTokenSource.Token);

        await task;

        //Assert
        task.IsCompleted.ShouldBeTrue();
        task.IsFaulted.ShouldBeFalse();
        
        job.ExecutionCount.ShouldBeGreaterThan(1);
    }
    
    [Test]
    public async Task ExecuteAsync_CronJobDoesThrow_ExecutedContinuously()
    {
        // Arrange
        var job = new TestJob(true);
        var sut = GetSut(job, "* * * * * *");
        var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.CancelAfter(TimeSpan.FromMilliseconds(100));

        // Act
        var task = sut.ExecuteAsync(cancellationTokenSource.Token);

        await task;
        
        cancellationTokenSource.Cancel();

        //Assert
        task.IsCompleted.ShouldBeTrue();
        task.IsFaulted.ShouldBeFalse();
        
        job.ExecutionCount.ShouldBe(0);
    }
    
    [Test]
    public async Task ExecuteAsync_FaultyCronExpression_ExecutedContinuously()
    {
        // Arrange
        var job = new TestJob(true);
        var sut = GetSut(job, "fail");
        var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.CancelAfter(TimeSpan.FromMilliseconds(100));

        // Act
        var task = sut.ExecuteAsync(cancellationTokenSource.Token);

        await task;
        
        cancellationTokenSource.Cancel();

        //Assert
        task.IsCompleted.ShouldBeTrue();
        task.IsFaulted.ShouldBeFalse();
        
        job.ExecutionCount.ShouldBe(0);
    }

    private CronHost GetSut(ICronJob testJob, string cronExpression)
    {
        var logger = Mock.Of<ILogger<CronHost>>();
        var cronWaiter = Mock.Of<ICronWaiter>();
        var jobProviderMock = new Mock<ICronJobProvider>();
        var optionsMonitorMock = new Mock<IOptionsMonitor<CronHostOptions>>();

        jobProviderMock
            .Setup(x => x.GetCronJob())
            .Returns(testJob);

        optionsMonitorMock
            .Setup(x => x.CurrentValue)
            .Returns(new CronHostOptions(){ CronString = cronExpression });

        return new CronHost(jobProviderMock.Object, cronWaiter, logger, optionsMonitorMock.Object);
    }
}

internal class TestJob : ICronJob
{
    private readonly bool shouldFail;

    public int ExecutionCount { get; private set; }

    public TestJob(bool shouldFail)
    {
        this.shouldFail = shouldFail;
    }

    public Task ExecuteAsync(CancellationToken cancellationToken)
    {
        if (shouldFail)
        {
            throw new InvalidOperationException("Failed explicitly");
        }

        ExecutionCount++;
        
        return Task.CompletedTask;
    }
}