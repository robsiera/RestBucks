using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;
using Application;
using Autofac;
using Autofac.Integration.WebApi;
using Infrastructure.HyperMedia.Linker;
using RestBucks.WebApi.ResourceProvider;

namespace RestBucks.WebApi.Modules
{
   public class ControllerResolver : IHttpControllerActivator
   {
      private readonly IContainer _container;

      private static void RegisterRequestDependantResources(ContainerBuilder containerBuilder, HttpRequestMessage request)
      {
         containerBuilder.RegisterType<ResourceLinkProvider>().AsImplementedInterfaces();
         containerBuilder.RegisterType<DtoMapper>().AsImplementedInterfaces();
         containerBuilder.RegisterApiControllers(typeof(WebApiApplication).Assembly);
         containerBuilder.RegisterInstance(new RouteLinker(request)).AsImplementedInterfaces();
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="T:System.Object"/> class.
      /// </summary>
      public ControllerResolver(IContainer container)
      {
         if (container == null)
         {
            throw new ArgumentNullException("container");
         }

         _container = container;
      }

      #region Implementation of IHttpControllerActivator

      /// <summary>
      /// Creates an <see cref="T:System.Web.Http.Controllers.IHttpController"/> object.
      /// </summary>
      /// <returns>
      /// An <see cref="T:System.Web.Http.Controllers.IHttpController"/> object.
      /// </returns>
      /// <param name="request">The message request.</param><param name="controllerDescriptor">The HTTP controller descriptor.</param><param name="controllerType">The type of the controller.</param>
      public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
      {
         var scope = _container.BeginLifetimeScope(x => RegisterRequestDependantResources(x, request));
         var controller = (IHttpController)scope.ResolveOptional(controllerType);
         request.RegisterForDispose(scope);
         return controller;
      }

      #endregion
   }
}