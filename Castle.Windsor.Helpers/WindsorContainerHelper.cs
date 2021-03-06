﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Castle.Core;
using Castle.Facilities.Startable;
using Castle.MicroKernel;
using Castle.MicroKernel.Context;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;

namespace Castle.Windsor.Helpers
{
    public static class WindsorContainerHelper
    {
        public static IWindsorContainer Register<TK, TV>(
            this IWindsorContainer container,
            Dictionary<string, object> dependencies = null,
            bool isTransient = false,
            Expression<Func<TK, Action>> startMethod = null,
            Expression<Func<TK, Action>> stopMethod = null,
            Action<TK> onCreate = null
            )
            where TK : class where TV : TK
        {
            var component = Component.For<TK>().ImplementedBy<TV>();

            if (dependencies == null)
            {
                return container.Register(component);
            }

            foreach (var key in dependencies.Keys)
            {
                component = component.DependsOn(Dependency.OnValue(key, dependencies[key]));
            }

            if (startMethod != null)
            {
                component = component.StartUsingMethod(startMethod);
            }

            if (stopMethod != null)
            {
                component = component.StopUsingMethod(stopMethod);
            }

            if (isTransient)
            {
                component = component.LifestyleTransient();
            }

            if (onCreate != null)
            {
                component = component.OnCreate(onCreate);
            }

            return container.Register(component);
        }

        public static IWindsorContainer Register<TV>(
            this IWindsorContainer container,
            Dictionary<string, object> dependencies = null,
            bool isTransient = false,
            Expression<Func<TV, Action>> startMethod = null,
            Expression<Func<TV, Action>> stopMethod = null,
            Action<TV> onCreate = null
        )
            where TV : class
        {
            var component = Component.For<TV>();

            if (dependencies == null)
            {
                return container.Register(component);
            }

            foreach (var key in dependencies.Keys)
            {
                component = component.DependsOn(Dependency.OnValue(key, dependencies[key]));
            }

            if (startMethod != null)
            {
                component = component.StartUsingMethod(startMethod);
            }

            if (stopMethod != null)
            {
                component = component.StopUsingMethod(stopMethod);
            }

            if (isTransient)
            {
                component = component.LifestyleTransient();
            }

            if (onCreate != null)
            {
                component = component.OnCreate(onCreate);
            }

            return container.Register(component);
        }

        public static IWindsorContainer RegisterUsingFactoryMethod<TV>(
            this IWindsorContainer container,
            Func<TV> factoryMethod,
            bool isTransient = false
        )
            where TV : class
        {
            var component = Component.For<TV>().UsingFactoryMethod(factoryMethod);

            if (isTransient)
            {
                component = component.LifestyleTransient();
            }

            return container.Register(component);
        }

        public static IWindsorContainer RegisterUsingFactoryMethod<TV>(
            this IWindsorContainer container,
            Func<IKernel, TV> factoryMethod,
            bool isTransient = false
        )
            where TV : class
        {
            var component = Component.For<TV>().UsingFactoryMethod(factoryMethod);

            if (isTransient)
            {
                component = component.LifestyleTransient();
            }

            return container.Register(component);
        }

        public static IWindsorContainer RegisterUsingFactoryMethod<TV>(
            this IWindsorContainer container,
            Func<IKernel, ComponentModel, CreationContext, TV> factoryMethod,
            bool isTransient = false
        )
            where TV : class
        {
            var component = Component.For<TV>().UsingFactoryMethod(factoryMethod);

            if (isTransient)
            {
                component = component.LifestyleTransient();
            }

            return container.Register(component);
        }

        public static IWindsorContainer RegisterUsingFactoryMethod<TV>(
            this IWindsorContainer container,
            Func<IKernel, CreationContext, TV> factoryMethod,
            bool isTransient = false
        )
            where TV : class
        {
            var component = Component.For<TV>().UsingFactoryMethod(factoryMethod);

            if (isTransient)
            {
                component = component.LifestyleTransient();
            }

            return container.Register(component);
        }

        public static void AddArraySubResolver(this IWindsorContainer container)
        {
            container.Kernel.Resolver.AddSubResolver(new ArrayResolver(container.Kernel, true));
        }
    }
}