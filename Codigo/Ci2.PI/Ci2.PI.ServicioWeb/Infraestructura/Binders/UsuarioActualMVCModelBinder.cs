using Ci2.PI.ServicioWeb.Entidades;
using Ci2.PI.ServicioWeb.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ci2.PI.ServicioWeb.Infraestructura.Binders
{
    public class UsuarioActualMVCModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            UsuarioActualMVC usuarioActual = new UsuarioActualMVC();


            if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.User != null && System.Web.HttpContext.Current.User.Identity != null)
            {
                string userId = System.Web.HttpContext.Current.User.Identity.GetUserId();

                if (userId != null && System.Web.HttpContext.Current.GetOwinContext() != null && System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>() != null)
                {
                    ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(userId);

                    usuarioActual = new UsuarioActualMVC()
                    {
                        UsuarioId = userId,
                        InformacionDeUsuarioComoApplicationUser = user
                    };
                }
            }

            return usuarioActual;
        }
    }
}