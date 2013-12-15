using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Net;
using ImdbRestService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ImdbRestServiceTests
{
    [TestClass]
    public class ImdbRestServiceTest
    {
        private ImdbRestWebServerAdapter _imdbRestWebServerAdapter;
        private Mock<ImdbRestWebServerAdapter.IRequest> _getRequestMock;
        private Mock<ImdbRestWebServerAdapter.IResponse> _responseMock;
        private Mock<ImdbRestWebServerAdapter.IRequest> _postRequestMock;
        private Mock<IPersistenceFacade> _persistenceFacade;

        [TestInitialize]
        public void Init()
        {
            _getRequestMock = new Mock<ImdbRestWebServerAdapter.IRequest>();
            _getRequestMock.Setup(x => x.HttpMethod).Returns("GET");
            _getRequestMock.Setup(x => x.RawUrl).Returns("movies/?title=");
            _postRequestMock = new Mock<ImdbRestWebServerAdapter.IRequest>();
            _responseMock = new Mock<ImdbRestWebServerAdapter.IResponse>();
            _persistenceFacade = new Mock<IPersistenceFacade>();

            _imdbRestWebServerAdapter = new ImdbRestWebServerAdapter(_persistenceFacade.Object);
        }

        [TestMethod]
        public void GettingExistingMovie()
        {
            _imdbRestWebServerAdapter.ProcessRequest(_getRequestMock.Object, _responseMock.Object);

            _getRequestMock.Verify(x => x.HttpMethod, Times.Exactly(1));
            _getRequestMock.Verify(x => x.RawUrl, Times.Exactly(1));

            Assert.AreEqual("", _imdbRestWebServerAdapter.ResponseData.Message);
        }
    }
}
