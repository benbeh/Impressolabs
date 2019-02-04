using System.Collections.Generic;
using System.Threading.Tasks;
using ImpressoApp.Services.RequestProvider;
using ImpressoApp.Models.Feeds;
using System;
using ImpressoApp.Constants;
using ImpressoApp.Models.Connect;

namespace ImpressoApp.Services.Feeds
{
    public class FeedsService : IFeedsService
    {
        private static readonly string ListEventsAndPeaopleEndpoint = ApplicationConstants.LiveServerApi + "/api/Connect/ListEventsAndPeople/";


        private readonly IRequestProvider requestProvider;

        public FeedsService(IRequestProvider requestProvider)
        {
            this.requestProvider = requestProvider;
        }

        public async Task<List<IConnectFeedModel>> GetConnects()
        {
            var connectAndEventsList = await requestProvider.GetAsync<ConnectAndEventsModel>(ListEventsAndPeaopleEndpoint);

            var connectsList = new List<IConnectFeedModel>();
            connectsList.AddRange(connectAndEventsList.People);
            connectsList.AddRange(connectAndEventsList.Events);

            return connectsList;
        }

        //public Task<List<IConnectFeedModel>> GetConnects()
        //{
        //    return Task.Run(() =>
        //    {
        //        var peopleConnect = new ConnectPeopleModel
        //        {
        //            Name = "Alen Owen",
        //            CityAddress = "Sofia, Bulgaria",
        //            PictureSource = "https://banner2.kisspng.com/20171218/f41/apple-logo-png-5a37e212dfda18.3311147015136117949169.jpg",
        //            Position = "Project Manager",
        //            CompanyPosition = "CEO at Wide Visions LTD",
        //            YearsOfExperiense = "19 Yrs Experience",
        //            Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. ",
        //        };
        //        var eventConnect = new ConnectEventModel
        //        {
        //            Dates = "Jun 23 - Jun 28 (UTC +02)",
        //            Address = "421 Stoltenberg Striven, London",
        //            Title = "Welcome To Free Classifieds Free Adds Free Advertisement",
        //            PictureSource = "event.png",
        //            DateNow = DateTime.Now,
        //            Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
        //            HostedBy = "Hosted by LCD ORGANIZATION NY",
        //            PrivacyType = "Public"
        //        };

        //        var resultList = new List<IConnectFeedModel>();
        //        resultList.Add(peopleConnect);
        //        resultList.Add(eventConnect);

        //        return resultList;
        //    });
        //}


    }
}
