using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BackOffice.Models;
using Forum.Business;
using System.Net;

namespace BackOffice.Controllers
{
    public class ForumController : Controller
    {
        // GET: Forum
        public ActionResult Index()
        {
            try
            {
                ForumBusiness forumB = new ForumBusiness();                
                return View(ConvertModel.ToModel(forumB.GetListForum()));
            }
            catch
            {
                return View();
            }
        }

        // GET: Forum/Details/5
        public ActionResult Details(int idForum)
        {
            try
            {
                ForumBusiness forumB = new ForumBusiness();
                ForumModel forumM = ConvertModel.ToModel(forumB.GetForum(idForum));
                return View(forumM);
            }
            catch
            {
                return View();
            }
        }

        // GET: Forum/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Forum/Create
        [HttpPost]
        public ActionResult Create(ForumModel forum)
        {
            try
            {
                // TODO: Add insert logic here
                ForumBusiness forumB = new ForumBusiness();
                forumB.CreateForum(ConvertModel.ToBusiness(forum));
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Forum/Edit/5
        public ActionResult Edit(int idForum)
        {
            ForumBusiness forumB = new ForumBusiness();
            return View(ConvertModel.ToModel(forumB.GetForum(idForum)));
        }

        // POST: Forum/Edit/5
        [HttpPost]
        public ActionResult Edit(ForumModel forum)
        {
            try
            {
                // TODO: Add update logic here
                ForumBusiness forumB = new ForumBusiness();
                forumB.EditForum(ConvertModel.ToBusiness(forum));
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // POST: Forum/Delete/5
        public ActionResult Delete(int idForum)
        {
            try
            {
                ForumBusiness forumB = new ForumBusiness();
                forumB.DeleteForum(idForum);
                return RedirectToAction("Index");
            }
            catch
            {
                return View("Index");
            }
        }
    }
}
