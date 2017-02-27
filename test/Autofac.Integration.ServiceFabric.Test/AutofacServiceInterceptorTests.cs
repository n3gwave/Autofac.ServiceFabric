﻿using Castle.DynamicProxy;
using Moq;
using Xunit;

namespace Autofac.Integration.ServiceFabric.Test
{
    public sealed class AutofacServiceInterceptorTests
    {
        [Theory]
        [InlineData("OnCloseAsync")]
        [InlineData("OnAbort")]
        public void DisposesLifetimeScopeWhenTriggerMethodInvoked(string methodName)
        {
            var lifetimeScope = new Mock<ILifetimeScope>(MockBehavior.Strict);
            lifetimeScope.Setup(x => x.Dispose()).Verifiable();

            var invocation = new Mock<IInvocation>(MockBehavior.Strict);
            invocation.Setup(x => x.Proceed()).Verifiable();
            invocation.Setup(x => x.Method.Name).Returns(methodName).Verifiable();

            var interceptor = new AutofacServiceInterceptor(lifetimeScope.Object);

            interceptor.Intercept(invocation.Object);

            lifetimeScope.Verify();
            invocation.Verify();
        }
    }
}
