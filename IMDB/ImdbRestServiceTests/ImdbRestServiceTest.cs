using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DtoSubsystem;
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
        private StreamWriter _streamWriter;

        [TestInitialize]
        public void Init()
        {
            Stream stream = new MemoryStream();
            _streamWriter = new StreamWriter(stream, Encoding.UTF8);
            _streamWriter.Write("test/test");
            _streamWriter.Flush();
            stream.Seek(0, SeekOrigin.Begin);

            _getRequestMock = new Mock<ImdbRestWebServerAdapter.IRequest>();
            _getRequestMock.Setup(x => x.HttpMethod).Returns("GET");


            _postRequestMock = new Mock<ImdbRestWebServerAdapter.IRequest>();
            _postRequestMock.Setup(x => x.HttpMethod).Returns("POST");
            _postRequestMock.Setup(x => x.InputStream).Returns(stream);

            _responseMock = new Mock<ImdbRestWebServerAdapter.IResponse>();
            _responseMock.Setup(x => x.OutputStream).Returns(stream);

            _persistenceFacade = new Mock<IPersistenceFacade>();
            _persistenceFacade.Setup(x => x.Get(It.IsAny<string>(), It.IsAny<Dto>())).Returns(Task.FromResult(new ResponseData("Success", HttpStatusCode.OK)));
            _persistenceFacade.Setup(x => x.Post(It.IsAny<string>(), It.IsAny<Dto>())).Returns(Task.FromResult(new ResponseData("Success", HttpStatusCode.OK)));

            _imdbRestWebServerAdapter = new ImdbRestWebServerAdapter(_persistenceFacade.Object);
        }

        [TestMethod]
        public void GettingSearchedMovies()
        {
            _getRequestMock.Setup(x => x.RawUrl).Returns("movies/?title=");

            Assert.IsNull(_imdbRestWebServerAdapter.ResponseData);

            _imdbRestWebServerAdapter.ProcessRequest(_getRequestMock.Object, _responseMock.Object);

            _getRequestMock.Verify(x => x.HttpMethod, Times.Exactly(1));
            _getRequestMock.Verify(x => x.RawUrl, Times.Exactly(1));

            Assert.AreEqual("Success", _imdbRestWebServerAdapter.ResponseData.Message);
        }

        [TestMethod]
        public void GettingSpecificMovie()
        {
            _getRequestMock.Setup(x => x.RawUrl).Returns("movies/1");

            Assert.IsNull(_imdbRestWebServerAdapter.ResponseData);

            _imdbRestWebServerAdapter.ProcessRequest(_getRequestMock.Object, _responseMock.Object);

            _getRequestMock.Verify(x => x.HttpMethod, Times.Exactly(1));
            _getRequestMock.Verify(x => x.RawUrl, Times.Exactly(1));

            Assert.AreEqual("Success", _imdbRestWebServerAdapter.ResponseData.Message);
        }

        [TestMethod]
        public void GettingSearchedPeople()
        {
            _getRequestMock.Setup(x => x.RawUrl).Returns("person/?person=");

            Assert.IsNull(_imdbRestWebServerAdapter.ResponseData);

            _imdbRestWebServerAdapter.ProcessRequest(_getRequestMock.Object, _responseMock.Object);

            _getRequestMock.Verify(x => x.HttpMethod, Times.Exactly(1));
            _getRequestMock.Verify(x => x.RawUrl, Times.Exactly(1));

            Assert.AreEqual("Success", _imdbRestWebServerAdapter.ResponseData.Message);
        }

        [TestMethod]
        public void GettingSpecificPerson()
        {
            _getRequestMock.Setup(x => x.RawUrl).Returns("person/1");

            Assert.IsNull(_imdbRestWebServerAdapter.ResponseData);

            _imdbRestWebServerAdapter.ProcessRequest(_getRequestMock.Object, _responseMock.Object);

            _getRequestMock.Verify(x => x.HttpMethod, Times.Exactly(1));
            _getRequestMock.Verify(x => x.RawUrl, Times.Exactly(1));

            Assert.AreEqual("Success", _imdbRestWebServerAdapter.ResponseData.Message);
        }

        [TestMethod]
        public void ReachingTestStub()
        {
            _getRequestMock.Setup(x => x.RawUrl).Returns("test");

            Assert.IsNull(_imdbRestWebServerAdapter.ResponseData);

            _imdbRestWebServerAdapter.ProcessRequest(_getRequestMock.Object, _responseMock.Object);

            _getRequestMock.Verify(x => x.HttpMethod, Times.Exactly(1));
            _getRequestMock.Verify(x => x.RawUrl, Times.Exactly(1));

            Assert.AreEqual("Success", _imdbRestWebServerAdapter.ResponseData.Message);
        }

        [TestMethod]
        public void BadResponseDataGet()
        {
            _getRequestMock.Setup(x => x.RawUrl).Returns("");

            Assert.IsNull(_imdbRestWebServerAdapter.ResponseData);

            _imdbRestWebServerAdapter.ProcessRequest(_getRequestMock.Object, _responseMock.Object);

            _getRequestMock.Verify(x => x.HttpMethod, Times.Exactly(1));
            _getRequestMock.Verify(x => x.RawUrl, Times.Exactly(1));

            Assert.AreEqual("Error: page not found", _imdbRestWebServerAdapter.ResponseData.Message);
        }

        [TestMethod]
        public void PostNewRegistration()
        {
            _postRequestMock.Setup(x => x.RawUrl).Returns("user/registration");
            
            Assert.IsNull(_imdbRestWebServerAdapter.ResponseData);

            _imdbRestWebServerAdapter.ProcessRequest(_postRequestMock.Object, _responseMock.Object);
            
            _postRequestMock.Verify(x => x.HttpMethod, Times.Exactly(2));
            _postRequestMock.Verify(x => x.RawUrl, Times.Exactly(1));

            Assert.AreEqual("Success", _imdbRestWebServerAdapter.ResponseData.Message);
        }

        [TestMethod]
        public void PostLogin()
        {
            _postRequestMock.Setup(x => x.RawUrl).Returns("user/login");

            Assert.IsNull(_imdbRestWebServerAdapter.ResponseData);

            _imdbRestWebServerAdapter.ProcessRequest(_postRequestMock.Object, _responseMock.Object);

            _postRequestMock.Verify(x => x.HttpMethod, Times.Exactly(2));
            _postRequestMock.Verify(x => x.RawUrl, Times.Exactly(1));

            Assert.AreEqual("Success", _imdbRestWebServerAdapter.ResponseData.Message);
        }


        [TestMethod]
        public void PostRating()
        {
            _postRequestMock.Setup(x => x.RawUrl).Returns("movies");

            Assert.IsNull(_imdbRestWebServerAdapter.ResponseData);

            _imdbRestWebServerAdapter.ProcessRequest(_postRequestMock.Object, _responseMock.Object);

            _postRequestMock.Verify(x => x.HttpMethod, Times.Exactly(2));
            _postRequestMock.Verify(x => x.RawUrl, Times.Exactly(1));

            Assert.AreEqual("Success", _imdbRestWebServerAdapter.ResponseData.Message);
        }

        [TestMethod]
        public void PostToTestStub()
        {
            _postRequestMock.Setup(x => x.RawUrl).Returns("test");

            Assert.IsNull(_imdbRestWebServerAdapter.ResponseData);

            _imdbRestWebServerAdapter.ProcessRequest(_postRequestMock.Object, _responseMock.Object);

            _postRequestMock.Verify(x => x.HttpMethod, Times.Exactly(2));
            _postRequestMock.Verify(x => x.RawUrl, Times.Exactly(1));

            Assert.AreEqual("Success", _imdbRestWebServerAdapter.ResponseData.Message);
        }

        [TestMethod]
        public void BadResponseDataPost()
        {
            _postRequestMock.Setup(x => x.RawUrl).Returns("");

            Assert.IsNull(_imdbRestWebServerAdapter.ResponseData);

            _imdbRestWebServerAdapter.ProcessRequest(_postRequestMock.Object, _responseMock.Object);

            _postRequestMock.Verify(x => x.HttpMethod, Times.Exactly(2));
            _postRequestMock.Verify(x => x.RawUrl, Times.Exactly(1));

            Assert.AreEqual("Empty path", _imdbRestWebServerAdapter.ResponseData.Message);
        }
    }
}
