using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Forum.Business;
using BackOffice.Models;
using RestSharp;

namespace BackOffice.Controllers
{
    public class MessageController : Controller
    {
        public T Execute<T>(RestRequest request) where T : new()
        {
            var client = new RestClient("http://aspmoduleprofil.azurewebsites.net/");
            var response = client.Execute<T>(request);
            return response.Data;
        }

        public UserSmallModel GetUserById(int id)
        {
            var request = new RestRequest("api/UserSmall/" + id, Method.GET);
            var result = Execute<UserSmallModel>(request);

            return result;
        }

        public bool PostMess(MessageModel Message, string pseudo)
        {
            var client = new RestClient("http://youp-recherche.azurewebsites.net/");
            RestRequest request = new RestRequest("update/get_postforum?id=" + Message.Message_id + "&date=" + Message.DatePoste + "&author=" + pseudo, Method.GET);
            var result = client.Execute<bool>(request);
            return result.Data;
        }

        // GET: Message
        public ActionResult Index()
        {
            try
            {
                MessageBusiness messageB = new MessageBusiness();
                List<MessageModel> list = ConvertModel.ToModel(messageB.GetListMessage());
                return View(list);
            }
            catch
            {
                return View();
            }
        }

        public ActionResult IndexByTopic(int idTopic)
        {
            try
            {
                MessageBusiness messageB = new MessageBusiness();
                List<MessageModel> list = ConvertModel.ToModel(messageB.GetListTopicMessage(idTopic));
                return View("Index", list);
            }
            catch
            {
                return View("Index");
            }
        }

        // GET: Message/Details/5
        public ActionResult Details(int idMessage)
        {
            try
            {
                MessageBusiness messageB = new MessageBusiness();
                MessageModel message = ConvertModel.ToModel(messageB.getMessage(idMessage));
                return View(message);
            }
            catch
            {
                return View();
            }
        }

        // GET: Message/Create
        public ActionResult Create()
        {
            TopicBusiness topicB = new TopicBusiness();
            List<TopicModel> list = ConvertModel.ToModel(topicB.GetListTopic());
            ViewBag.TopicChoice = new SelectList(list, "Topic_id", "Nom");
            return View();
        }

        // POST: Message/Create
        [HttpPost]
        public ActionResult Create(MessageModel message, DateTime DateCrea, int TopicChoice)
        {
            message.Topic_id = TopicChoice;
            message.DatePoste = DateCrea;
            try
            {
                // TODO: Add insert logic here

                MessageBusiness messageB = new MessageBusiness();
                messageB.CreateMessage(ConvertModel.ToBusiness(message));

                UserSmallModel user = this.GetUserById(Convert.ToInt32(message.Utilisateur_id));
                MessageModel mes = ConvertModel.ToModel(messageB.GetListMessage().OrderBy(o => o.Message_id).LastOrDefault());
                bool postmes = this.PostMess(mes, user.Pseudo);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Message/Edit/5
        public ActionResult Edit(int idMessage)
        {
            MessageBusiness messageB = new MessageBusiness();
            return View(ConvertModel.ToModel(messageB.getMessage(idMessage)));
        }

        // POST: Message/Edit/5
        [HttpPost]
        public ActionResult Edit(MessageModel message, DateTime DateCrea)
        {
            message.DatePoste = DateCrea;
            try
            {
                // TODO: Add update logic here

                MessageBusiness messageB = new MessageBusiness();
                messageB.EditMessage(ConvertModel.ToBusiness(message));

                UserSmallModel user = this.GetUserById(Convert.ToInt32(message.Utilisateur_id));                
                bool postmes = this.PostMess(message, user.Pseudo);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // POST: Message/Delete/5
        public ActionResult Delete(int idMessage)
        {
            try
            {
                // TODO: Add delete logic here

                MessageBusiness messageB = new MessageBusiness();
                MessageModel mesSup = ConvertModel.ToModel(messageB.getMessage(idMessage));
                messageB.DeleteMessage(idMessage);

                UserSmallModel user = this.GetUserById(Convert.ToInt32(mesSup.Utilisateur_id));
                bool postmes = this.PostMess(mesSup, user.Pseudo);

                return RedirectToAction("Index");
            }
            catch
            {
                return View("Index");
            }
        }
    }
}
