using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.PolicyInjection;

using Nega.WcfUnity;
using Nega.Entlib;

using Zero.Service.Rest;
using Zero.BLL;
using Zero.BLL.Managers;
using Zero.DAL;
using Zero.DAL.EF.MySQL;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging;

namespace Zero.Service.IISWASHost
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            Console.WriteLine("app start");

            IUnityContainer container = ObjectsRegistry.SoloInstance.Container;

            container.AddNewExtension<Interception>();

            container.Configure<Interception>().AddPolicy("Save")
                 .AddMatchingRule<MemberNameMatchingRule>(new InjectionConstructor(new InjectionParameter("Save*")))
                 .AddCallHandler<TransactionCallHandler>(new ContainerControlledLifetimeManager(), new InjectionConstructor());
            container.Configure<Interception>().AddPolicy("Update")
                 .AddMatchingRule<MemberNameMatchingRule>(new InjectionConstructor(new InjectionParameter("Update*")))
                 .AddCallHandler<TransactionCallHandler>(new ContainerControlledLifetimeManager(), new InjectionConstructor());

            container.Configure<Interception>().AddPolicy("Save2")
                 .AddMatchingRule<MemberNameMatchingRule>(new InjectionConstructor(new InjectionParameter("Save*")))
                 .AddCallHandler<ExceptionCallHandler>(new ContainerControlledLifetimeManager(), new InjectionConstructor("LogAndWrap"));

            container.RegisterType<IUserDao, UserDao>();
            container.RegisterType<IRoleDao, RoleDao>();

            container.RegisterType<IUserService, UserManager>(new Interceptor<TransparentProxyInterceptor>(),
                new InterceptionBehavior<PolicyInjectionBehavior>());

            container.RegisterType<ISignService, SignService>();

            PolicyInjection.SetPolicyInjector(new PolicyInjector(new SystemConfigurationSource(false)), false);


            LoggingConfiguration loggingConfiguration = BuildLoggingConfig();
            LogWriter logWriter = new LogWriter(loggingConfiguration);
            //Logger.SetLogWriter(logWriter, false);

            // Create the default ExceptionManager object from the configuration settings.
            //ExceptionPolicyFactory policyFactory = new ExceptionPolicyFactory();
            //ExceptionManager exManager = policyFactory.CreateManager();

            // Create the default ExceptionManager object programatically
            ExceptionManager exManager = BuildExceptionManagerConfig(logWriter);

            // Create an ExceptionPolicy to illustrate the static HandleException method
            ExceptionPolicy.SetExceptionManager(exManager);

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }



        private static ExceptionManager BuildExceptionManagerConfig(LogWriter logWriter)
        {
            var policies = new List<ExceptionPolicyDefinition>();

            var assistingAdministrators = new List<ExceptionPolicyEntry>
            {
                new ExceptionPolicyEntry(typeof (Exception),
                    PostHandlingAction.ThrowNewException,
                    new IExceptionHandler[]
                     {
                       new LoggingExceptionHandler("General", 9001, TraceEventType.Error,
                         "Salary Calculations Service", 5, typeof(TextExceptionFormatter), logWriter),
                       new ReplaceHandler("Application error.  Please advise your administrator and provide them with this error code: {handlingInstanceID}",
                         typeof(Exception))
                     })
            };

            var exceptionShielding = new List<ExceptionPolicyEntry>
            {
                new ExceptionPolicyEntry(typeof (Exception),
                    PostHandlingAction.ThrowNewException,
                    new IExceptionHandler[]
                     {
                       new WrapHandler("Application Error. Please contact your administrator.",
                         typeof(Exception))
                     })
            };

            var loggingAndReplacing = new List<ExceptionPolicyEntry>
            {
                new ExceptionPolicyEntry(typeof (Exception),
                    PostHandlingAction.ThrowNewException,
                    new IExceptionHandler[]
                     {
                       new LoggingExceptionHandler("General", 9000, TraceEventType.Error,
                         "Salary Calculations Service", 5, typeof(TextExceptionFormatter), logWriter),
                       new ReplaceHandler("An application error occurred and has been logged. Please contact your administrator.",
                         typeof(Exception))
                     })
            };

            var logAndWrap = new List<ExceptionPolicyEntry>
            {
                new ExceptionPolicyEntry(typeof (DivideByZeroException),
                    PostHandlingAction.None,
                    new IExceptionHandler[]
                     {
                       new LoggingExceptionHandler("General", 9002, TraceEventType.Error,
                         "Salary Calculations Service", 5, typeof(TextExceptionFormatter), logWriter),
                       new ReplaceHandler("Application error will be ignored and processing will continue.",
                         typeof(Exception))
                     }),
                new ExceptionPolicyEntry(typeof (Exception),
                    PostHandlingAction.ThrowNewException,
                    new IExceptionHandler[]
                     {
                       new WrapHandler("An application error has occurred.",
                         typeof(Exception))
                     })
            };

            var replacingException = new List<ExceptionPolicyEntry>
            {
                new ExceptionPolicyEntry(typeof (Exception),
                    PostHandlingAction.ThrowNewException,
                    new IExceptionHandler[]
                     {
                       new ReplaceHandler("Application Error. Please contact your administrator.",
                         typeof(Exception))
                     })
            };

            policies.Add(new ExceptionPolicyDefinition("AssistingAdministrators", assistingAdministrators));
            policies.Add(new ExceptionPolicyDefinition("ExceptionShielding", exceptionShielding));
            policies.Add(new ExceptionPolicyDefinition("LoggingAndReplacingException", loggingAndReplacing));
            policies.Add(new ExceptionPolicyDefinition("LogAndWrap", logAndWrap));
            policies.Add(new ExceptionPolicyDefinition("ReplacingException", replacingException));
            return new ExceptionManager(policies);
        }

        private static LoggingConfiguration BuildLoggingConfig()
        {
            // Formatters
            TextFormatter formatter = new TextFormatter("Timestamp: {timestamp}{newline}Message: {message}{newline}Category: {category}{newline}Priority: {priority}{newline}EventId: {eventid}{newline}Severity: {severity}{newline}Title:{title}{newline}Machine: {localMachine}{newline}App Domain: {localAppDomain}{newline}ProcessId: {localProcessId}{newline}Process Name: {localProcessName}{newline}Thread Name: {threadName}{newline}Win32 ThreadId:{win32ThreadId}{newline}Extended Properties: {dictionary({key} - {value}{newline})}");

            // Listeners
            var flatFileTraceListener = new FlatFileTraceListener(@"d:\Temp\SalaryCalculator.log", "----------------------------------------", "----------------------------------------", formatter);
            var eventLog = new EventLog("Application", ".", "Enterprise Library Logging");
            var eventLogTraceListener = new FormattedEventLogTraceListener(eventLog);
            // Build Configuration
            var config = new LoggingConfiguration();
            config.AddLogSource("General", SourceLevels.All, true).AddTraceListener(eventLogTraceListener);
            config.LogSources["General"].AddTraceListener(flatFileTraceListener);

            // Special Sources Configuration
            config.SpecialSources.LoggingErrorsAndWarnings.AddTraceListener(eventLogTraceListener);

            return config;
        }

    }
}