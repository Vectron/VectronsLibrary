using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using VectronsLibrary.DI.TestsAssembly;

namespace VectronsLibrary.DI.Tests
{
    [TestClass]
    public class IServiceCollectionExtensionTests
    {
        [TestMethod]
        public void AddAssemblyResolverAddsResolverToCollection()
        {
            // Arrange
            var collection = new ServiceCollection();

            // Act
            collection.AddAssemblyResolver();

            // Assert
            Assert.AreEqual(2, collection.Count);
            ValidateServiceDescriptor(collection[0], ServiceLifetime.Singleton, typeof(IAssemblyResolver), typeof(AssemblyResolver));
            ValidateServiceDescriptor(collection[1], ServiceLifetime.Singleton, typeof(AssemblyResolver), typeof(AssemblyResolver));
        }

        [TestMethod]
        public void AddByAttributeEnumerableTest()
        {
            // Arrange
            var collection = new ServiceCollection();
            var items = new List<Type>()
            {
                typeof(NoAttributeClass),
                typeof(SingletonClass),
                typeof(ScopedClass),
                typeof(TransientClass)
            };

            // Act
            collection.AddByAttribute(items, typeof(IAttributeClass));

            // Assert
            Assert.AreEqual(8, collection.Count);
            ValidateServiceDescriptor(collection[0], ServiceLifetime.Scoped, typeof(IAttributeClass), typeof(NoAttributeClass));
            ValidateServiceDescriptor(collection[1], ServiceLifetime.Scoped, typeof(NoAttributeClass), typeof(NoAttributeClass));
            ValidateServiceDescriptor(collection[2], ServiceLifetime.Singleton, typeof(IAttributeClass), typeof(SingletonClass));
            ValidateServiceDescriptor(collection[3], ServiceLifetime.Singleton, typeof(SingletonClass), typeof(SingletonClass));
            ValidateServiceDescriptor(collection[4], ServiceLifetime.Scoped, typeof(IAttributeClass), typeof(ScopedClass));
            ValidateServiceDescriptor(collection[5], ServiceLifetime.Scoped, typeof(ScopedClass), typeof(ScopedClass));
            ValidateServiceDescriptor(collection[6], ServiceLifetime.Transient, typeof(IAttributeClass), typeof(TransientClass));
            ValidateServiceDescriptor(collection[7], ServiceLifetime.Transient, typeof(TransientClass), typeof(TransientClass));
        }

        [TestMethod]
        public void AddByAttributeTest()
        {
            // Arrange
            var collection = new ServiceCollection();

            // Act
            collection.AddByAttribute(typeof(NoAttributeClass), typeof(IAttributeClass));
            collection.AddByAttribute(typeof(SingletonClass), typeof(IAttributeClass));
            collection.AddByAttribute(typeof(ScopedClass), typeof(IAttributeClass));
            collection.AddByAttribute(typeof(TransientClass), typeof(IAttributeClass));

            collection.AddByAttribute(typeof(NoAttributeClass2), typeof(NoAttributeClass2));
            collection.AddByAttribute(typeof(SingletonClass2), typeof(SingletonClass2));
            collection.AddByAttribute(typeof(ScopedClass2), typeof(ScopedClass2));
            collection.AddByAttribute(typeof(TransientClass2), typeof(TransientClass2));

            // Assert
            Assert.AreEqual(12, collection.Count);
            ValidateServiceDescriptor(collection[0], ServiceLifetime.Scoped, typeof(IAttributeClass), typeof(NoAttributeClass));
            ValidateServiceDescriptor(collection[1], ServiceLifetime.Scoped, typeof(NoAttributeClass), typeof(NoAttributeClass));
            ValidateServiceDescriptor(collection[2], ServiceLifetime.Singleton, typeof(IAttributeClass), typeof(SingletonClass));
            ValidateServiceDescriptor(collection[3], ServiceLifetime.Singleton, typeof(SingletonClass), typeof(SingletonClass));
            ValidateServiceDescriptor(collection[4], ServiceLifetime.Scoped, typeof(IAttributeClass), typeof(ScopedClass));
            ValidateServiceDescriptor(collection[5], ServiceLifetime.Scoped, typeof(ScopedClass), typeof(ScopedClass));
            ValidateServiceDescriptor(collection[6], ServiceLifetime.Transient, typeof(IAttributeClass), typeof(TransientClass));
            ValidateServiceDescriptor(collection[7], ServiceLifetime.Transient, typeof(TransientClass), typeof(TransientClass));

            ValidateServiceDescriptor(collection[8], ServiceLifetime.Scoped, typeof(NoAttributeClass2), typeof(NoAttributeClass2));
            ValidateServiceDescriptor(collection[9], ServiceLifetime.Singleton, typeof(SingletonClass2), typeof(SingletonClass2));
            ValidateServiceDescriptor(collection[10], ServiceLifetime.Scoped, typeof(ScopedClass2), typeof(ScopedClass2));
            ValidateServiceDescriptor(collection[11], ServiceLifetime.Transient, typeof(TransientClass2), typeof(TransientClass2));
        }

        [TestMethod]
        public void AddFromAssemblies()
        {
            // Arrange
            var collection = new ServiceCollection();
            var assemblies = new List<string>()
            {
                "VectronsLibrary.DI.TestsAssembly.dll"
            };

            // Act
            collection.AddFromAssemblies(assemblies);

            // Assert
            Assert.AreEqual(10, collection.Count);
        }

        [TestMethod]
        public void AddNonGenericLoggerErrorTest()
        {
            // Arrange
            var collection = new ServiceCollection();

            // Act
            collection.AddNonGenericLoggerError();
            var provider = collection.BuildServiceProvider();

            // Assert
            Assert.AreEqual(1, collection.Count);
            ValidateServiceDescriptor(collection[0], ServiceLifetime.Singleton, typeof(ILogger), null);
            Assert.ThrowsException<NotImplementedException>(() => provider.GetService<ILogger>());
        }

        [TestMethod]
        public void AddRegisteredTypesTest()
        {
            // Arrange
            var collection = new ServiceCollection();

            // Act
            collection.AddRegisteredTypes();

            // Assert
            Assert.AreEqual(2, collection.Count);
            ValidateServiceDescriptor(collection[1], ServiceLifetime.Singleton, typeof(IRegisteredTypes<>), typeof(RegisteredTypes<>));
        }

        [TestMethod]
        public void ScopedIsResolvedInEveryScope()
        {
            // Arrange
            var provider = new ServiceCollection()
                .AddByAttribute(typeof(ScopedClass), typeof(IAttributeClass))
                .BuildServiceProvider();

            // Act
            var first = provider.GetService<IAttributeClass>();
            var second = provider.GetService<IAttributeClass>();
            var notByInterface = provider.GetService<ScopedClass>();

            var scope = provider.CreateScope();
            var third = scope.ServiceProvider.GetService<IAttributeClass>();
            var fourth = scope.ServiceProvider.GetService<IAttributeClass>();
            var notByInterfaceScoped = scope.ServiceProvider.GetService<ScopedClass>();

            // Assert
            Assert.AreSame(first, second, "first and second");
            Assert.AreNotSame(first, third, "first and third");
            Assert.AreSame(third, fourth, "third and fourth");
            Assert.AreSame(first, notByInterface, "first and notByInterface");
            Assert.AreSame(third, notByInterfaceScoped, "third and notByInterfaceScoped");
            Assert.AreNotSame(notByInterface, third, "notByInterface and notByInterfaceScoped");
        }

        [TestMethod]
        public void SingletonOnlyResolvedOnce()
        {
            // Arrange
            var provider = new ServiceCollection()
                .AddByAttribute(typeof(SingletonClass), typeof(IAttributeClass))
                .BuildServiceProvider();

            // Act
            var first = provider.GetService<IAttributeClass>();
            var second = provider.GetService<IAttributeClass>();
            var notByInterface = provider.GetService<SingletonClass>();

            var scope = provider.CreateScope();
            var third = scope.ServiceProvider.GetService<IAttributeClass>();
            var fourth = scope.ServiceProvider.GetService<IAttributeClass>();
            var notByInterfaceScoped = provider.GetService<SingletonClass>();

            // Assert
            Assert.AreSame(first, second, "first and second");
            Assert.AreSame(first, third, "first and third");
            Assert.AreSame(first, fourth, "first and fourth");
            Assert.AreSame(first, notByInterface, "first and notByInterface");
        }

        [TestMethod]
        public void TransientIsResolvedEveryTime()
        {
            // Arrange
            var provider = new ServiceCollection()
                .AddByAttribute(typeof(TransientClass), typeof(IAttributeClass))
                .BuildServiceProvider();

            // Act
            var first = provider.GetService<IAttributeClass>();
            var second = provider.GetService<IAttributeClass>();
            var notByInterface = provider.GetService<TransientClass>();

            var scope = provider.CreateScope();
            var third = scope.ServiceProvider.GetService<IAttributeClass>();
            var fourth = scope.ServiceProvider.GetService<IAttributeClass>();

            // Assert
            Assert.AreNotSame(first, second, "first and second");
            Assert.AreNotSame(first, third, "first and third");
            Assert.AreNotSame(first, fourth, "first and fourth");

            Assert.AreNotSame(second, third, "second and third");
            Assert.AreNotSame(second, fourth, "second and fourth");

            Assert.AreNotSame(third, fourth, "third and fourth");

            Assert.AreNotSame(first, notByInterface, "first and notByInterface");
        }

        [TestMethod]
        public void TryAddByAttributeTest()
        {
            // Arrange
            var collection = new ServiceCollection();

            // Act
            collection.TryAddByAttribute(typeof(NoAttributeClass), typeof(IAttributeClass));
            collection.TryAddByAttribute(typeof(SingletonClass), typeof(IAttributeClass));
            collection.TryAddByAttribute(typeof(ScopedClass), typeof(IAttributeClass));
            collection.TryAddByAttribute(typeof(TransientClass), typeof(IAttributeClass));

            collection.TryAddByAttribute(typeof(NoAttributeClass2), typeof(NoAttributeClass2));
            collection.TryAddByAttribute(typeof(SingletonClass2), typeof(SingletonClass2));
            collection.TryAddByAttribute(typeof(ScopedClass2), typeof(ScopedClass2));
            collection.TryAddByAttribute(typeof(TransientClass2), typeof(TransientClass2));

            collection.TryAddByAttribute(typeof(NoAttributeClass), typeof(IAttributeClass));
            collection.TryAddByAttribute(typeof(SingletonClass), typeof(IAttributeClass));
            collection.TryAddByAttribute(typeof(ScopedClass), typeof(IAttributeClass));
            collection.TryAddByAttribute(typeof(TransientClass), typeof(IAttributeClass));

            collection.TryAddByAttribute(typeof(NoAttributeClass2), typeof(NoAttributeClass2));
            collection.TryAddByAttribute(typeof(SingletonClass2), typeof(SingletonClass2));
            collection.TryAddByAttribute(typeof(ScopedClass2), typeof(ScopedClass2));
            collection.TryAddByAttribute(typeof(TransientClass2), typeof(TransientClass2));

            // Assert
            Assert.AreEqual(9, collection.Count);
            ValidateServiceDescriptor(collection[0], ServiceLifetime.Scoped, typeof(IAttributeClass), typeof(NoAttributeClass));
            ValidateServiceDescriptor(collection[1], ServiceLifetime.Scoped, typeof(NoAttributeClass), typeof(NoAttributeClass));
            ValidateServiceDescriptor(collection[2], ServiceLifetime.Singleton, typeof(SingletonClass), typeof(SingletonClass));
            ValidateServiceDescriptor(collection[3], ServiceLifetime.Scoped, typeof(ScopedClass), typeof(ScopedClass));
            ValidateServiceDescriptor(collection[4], ServiceLifetime.Transient, typeof(TransientClass), typeof(TransientClass));

            ValidateServiceDescriptor(collection[5], ServiceLifetime.Scoped, typeof(NoAttributeClass2), typeof(NoAttributeClass2));
            ValidateServiceDescriptor(collection[6], ServiceLifetime.Singleton, typeof(SingletonClass2), typeof(SingletonClass2));
            ValidateServiceDescriptor(collection[7], ServiceLifetime.Scoped, typeof(ScopedClass2), typeof(ScopedClass2));
            ValidateServiceDescriptor(collection[8], ServiceLifetime.Transient, typeof(TransientClass2), typeof(TransientClass2));
        }

        private void ValidateServiceDescriptor(ServiceDescriptor descriptor, ServiceLifetime lifetime, Type serviceType, Type ImplementationType)
        {
            Assert.IsTrue(descriptor.Lifetime == lifetime);
            Assert.AreEqual(serviceType, descriptor.ServiceType);

            if (descriptor.ImplementationFactory == null)
            {
                Assert.AreEqual(ImplementationType, descriptor.ImplementationType);
            }
            else
            {
                Assert.IsNull(descriptor.ImplementationType);
            }
        }
    }
}