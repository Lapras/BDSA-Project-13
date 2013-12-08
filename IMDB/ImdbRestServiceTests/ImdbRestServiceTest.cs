using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Net;
using ImdbRestService;
using ImdbRestService.Handlers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ImdbRestServiceTests
{
    [TestClass]
    public class ImdbRestServiceTest
    {
        private FakeImdbEntities _entities;
        private MovieHandler _handler;
        private ImdbRestWebServer _restServer;

        [TestInitialize]
        public void Init()
        {
            
            _entities = new FakeImdbEntities
            {
                Movies =
				{
					new Movie {Id = 0, Title = "Title", Year = 2013}
				}
            };
            _handler = new MovieHandler(_entities);
        }

        [TestMethod]
        public void GettingExistingMovie()
        {           
            var responseData = new ResponseData("", HttpStatusCode.BadRequest);
            responseData = _handler.Handle(new List<string> { "?title=Title" }, responseData).Result;

            Assert.AreEqual(responseData.HttpStatusCode, HttpStatusCode.OK);
        }

        [TestMethod]
        public void TryingToGetNonExistingMovie()
        {
            var responseData = new ResponseData("", HttpStatusCode.OK);
            responseData = _handler.Handle(new List<string> { "?title=NoMovie" }, responseData).Result;

            Assert.AreEqual(responseData.HttpStatusCode, HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public void CorrectHandlerInvoked()
        {
            var movieHandlerMock = new Mock<MovieHandler>();
            var profileHandlerMock = new Mock<ProfileHandler>();
            var handlerList = new List<IHandler>{ movieHandlerMock.Object, profileHandlerMock.Object};
            //var context = new HttpListenerContext(new HttpListener(), null);
            _restServer = new ImdbRestWebServer();
            //_restServer.ProcessRequest();
        }
    }


    public class FakeDbSet<T> : IDbSet<T>
    where T : class
    {
        ObservableCollection<T> _data;
        IQueryable _query;

        public FakeDbSet()
        {
            _data = new ObservableCollection<T>();
            _query = _data.AsQueryable();
        }

        public virtual T Find(params object[] keyValues)
        {
            throw new NotImplementedException("Derive from FakeDbSet<T> and override Find");
        }

        public T Add(T item)
        {
            _data.Add(item);
            return item;
        }

        public T Remove(T item)
        {
            _data.Remove(item);
            return item;
        }

        public T Attach(T item)
        {
            _data.Add(item);
            return item;
        }

        public T Detach(T item)
        {
            _data.Remove(item);
            return item;
        }

        public T Create()
        {
            return Activator.CreateInstance<T>();
        }

        public TDerivedEntity Create<TDerivedEntity>() where TDerivedEntity : class, T
        {
            return Activator.CreateInstance<TDerivedEntity>();
        }

        public ObservableCollection<T> Local
        {
            get { return _data; }
        }

        Type IQueryable.ElementType
        {
            get { return _query.ElementType; }
        }

        System.Linq.Expressions.Expression IQueryable.Expression
        {
            get { return _query.Expression; }
        }

        IQueryProvider IQueryable.Provider
        {
            get { return _query.Provider; }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return _data.GetEnumerator();
        }
    }

    public class FakeCategorySet : FakeDbSet<Movie>
    {
        public override Movie Find(params object[] keyValues)
        {
            return this.SingleOrDefault(d => d.Title == (string)keyValues.Single());
        }
    }


    public class FakeImdbEntities : IImdbEntities
    {
        public FakeImdbEntities()
        {
            Movies = new FakeCategorySet();
        }

        public IDbSet<Movie> Movies { get; private set; }
        
        public void Dispose()
        {
            
        }
    }

    //    public int SaveChanges()
        //    {
        //        return 0;
        //    }

    }
