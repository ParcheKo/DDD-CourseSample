﻿using Castle.DynamicProxy;
using Framework.Application;
using Framework.Core;
using System;
using System.Reflection;

namespace Framework.Config
{
    public class LoggingInterceptor : IInterceptor
    {
       private ILogger _logger;
        private ISerializer _serializer;
        public LoggingInterceptor(ILogger logger, ISerializer serializer)
        {
            _logger = logger;
            _serializer = serializer;
        }

        public void Intercept(IInvocation invocation)
        {
            var logData = new LogData("Admin", "192.168.10.22", $"Begin - {invocation.Arguments[0].GetType().Name}",_serializer.Serialize(invocation.Arguments[0]), Guid.NewGuid());
            _logger.Log(logData);
            invocation.Proceed();
            logData.Data = $"End - { invocation.Arguments[0].GetType().Name}";
          
            _logger.Log(logData);

        }
    }

    public class CommndHandlerLogHook : IProxyGenerationHook
    {
        public void MethodsInspected()
        {
           // throw new NotImplementedException();
        }

        public void NonProxyableMemberNotification(Type type, MemberInfo memberInfo)
        {
          //  throw new NotImplementedException();
        }

        public bool ShouldInterceptMethod(Type type, MethodInfo methodInfo)
        {
            return type == typeof(ICommandHandler) && methodInfo.Name == "Handle";
        }
    }
}
