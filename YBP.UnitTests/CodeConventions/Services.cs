using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace YBP.UnitTests.CodeConventions
{
    [TestClass]
    public class Services
    {
        private readonly Type[] _services;
        private readonly Assembly[] _ownAsemblies;

        private Type[] _exceptions = new[] {
            typeof(Sample.Definitions.Common.RuleViolations)
        };

        public Services()
        {
            _services = Assembly
                .GetAssembly(typeof(Sample.BP.Configuration))
                .GetTypes()
                .Where(x => x.Namespace.StartsWith("Sample.Services") && !x.Name.EndsWith("Info"))
                .ToArray();

            _ownAsemblies = new[] {
                Assembly.GetAssembly(typeof(Sample.BP.Configuration)),
                Assembly.GetAssembly(typeof(Sample.Definitions.Constants)),
                Assembly.GetAssembly(typeof(Sample.Data.SampleDbContext))
            };
        }


        [TestInitialize]
        public void Init()
        {
        }

        [TestMethod]
        public void Should_not_have_writable_properites()
        {
            foreach (var service in _services)
            {
                var properties = service
                    .GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)
                    .Where(x => x.CanWrite)
                    .Select(x => x.Name)
                    .ToArray();

                if (properties.Any())
                {
                    var props = string.Join(", ", properties);
                    throw new ApplicationException($"Service {service.FullName} is not stateless becouse it has writable properties: {props}");
                }

            }
        }

        [TestMethod]
        public void Should_not_have_writable_fields()
        {
            foreach (var service in _services)
            {
                var fields = service
                    .GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)
                    .Where(x => !x.IsInitOnly && !x.Name.StartsWith('<'))
                    .Select(x => x.Name)
                    .ToArray();

                if (fields.Any())
                {
                    var names = string.Join(", ", fields);
                    throw new ApplicationException($"Service {service.FullName} is not stateless becouse it has writable fields: {names}");
                }

            }
        }

        [TestMethod]
        public void Should_not_have_parameters_with_executable_code()
        {
            foreach (var service in _services)
            {
                var methods = service.GetMethods();

                foreach (var method in methods)
                {
                    CheckHasExecutableCode(method.ReturnType, $"Method {method.Name} of {service.FullName} service returns type with executable code.");

                    var args = method.GetParameters();

                    foreach (var arg in args)
                    {
                        CheckHasExecutableCode(method.ReturnType, $"Method {method.Name} of {service.FullName} service uses {arg.Name} parameter of type with executable code.");
                    }
                }
            }
        }

        private void CheckHasExecutableCode(Type type, string errorMessage)
        {
            if (_ownAsemblies.Contains(type.Assembly))
            {
                if (!CheckType(type))
                    throw new ApplicationException(errorMessage);

                return;
            }

            if (type.FullName.StartsWith("System.Linq.IQueryable"))
            {
                throw new ApplicationException($"{errorMessage} IQueryable is not allowed for service parameters");
            }

        }

        private bool CheckType(Type type)
        {
            if (_exceptions.Contains(type))
                return true;

            if (type.Namespace == "System")
                return true;

            if (type.IsGenericType && type.Namespace.StartsWith("System.Linq"))
            {
                var paramz = type.GetGenericArguments();
                foreach (var p in paramz)
                    if (!CheckType(p))
                    {
                        return false;
                    }
            }

            if (type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Any())
            {
                return false;
            }

            var properties = type.GetProperties();
            foreach (var prop in properties)
            {
                if (!CheckType(prop.PropertyType))
                    return false;
            }

            if (type.IsGenericType)
            {
                var genericParams = type.GetGenericArguments();

                foreach (var genericParam in genericParams)
                {
                    if (!CheckType(genericParam))
                        return false;
                }
            }

            return true;
        }
    }
}
