using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DtoSubsystem;
using ImdbRestService;
using ImdbRestService.ImdbRepositories;
using ImdbRestService.Mapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ImdbRestServiceTests
{
    [TestClass]
    public class MovieMapperTest
    {
        private FakeImdbEntities _imdbEntities;
        private MovieMapper _movieMapper;
        private Mock<IExternalMovieDatabaseRepository> _externalMovieDatabaseRepository;
        private FakeImdbEntities _unavailableDatabase;

        [TestInitialize]
        public void Init()
        {
            _externalMovieDatabaseRepository = new Mock<IExternalMovieDatabaseRepository>();
            _externalMovieDatabaseRepository.Setup(x => x.GetMoviesFromImdbAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(new List<MovieDto>()
                {
                    new MovieDto()
                    {
                        Title = "Successfully searched external database"
                    }
                }));

            _imdbEntities = InitializeImdbEntities();

            _unavailableDatabase = new FakeImdbEntities
            {
                Movies = {new Movie(){Title = null}}
            };
            _movieMapper = new MovieMapper(_imdbEntities, _externalMovieDatabaseRepository.Object);
        }



        [TestMethod]
        public void SearchingExistingMovies()
        {
            const string searchString = "Title";
            var resultList = _movieMapper.GetMoviesByTitle(searchString);

            Assert.AreEqual(3, resultList.Count);
        }

        [TestMethod]
        public void SearchingNonExistingMovies()
        {
            const string searchString = "NoResults";
            var resultList = _movieMapper.GetMoviesByTitle(searchString);

            Assert.AreEqual(0, resultList.Count);
        }

        [TestMethod]
        public void SearchMoviesDatabaseNotAvailable()
        {
            _movieMapper = new MovieMapper(_unavailableDatabase, _externalMovieDatabaseRepository.Object);

            const string searchString = "SearchExternalDatabase";
            var resultList = _movieMapper.GetMoviesByTitle(searchString);

            Assert.AreEqual("Successfully searched external database", resultList.First().Title);
        }


        private FakeImdbEntities InitializeImdbEntities()
        {
            return new FakeImdbEntities
            {
                Movies =
                {
                    new Movie
                    {
                        Id = 1,
                        Title = "TitleOne",
                        Year = 2001,
                        EpisodeNumber = 1,
                        EpisodeOf_Id = 1,
                        Kind = "Kind",
                        SeasonNumber = 1,
                        SeriesYear = "2001"
                    },
                    new Movie
                    {
                        Id = 2,
                        Title = "TitleTwo",
                        Year = 2002,
                        EpisodeNumber = 1,
                        EpisodeOf_Id = 1,
                        Kind = "Kind",
                        SeasonNumber = 1,
                        SeriesYear = "2002"
                    },
                    new Movie
                    {
                        Id = 3,
                        Title = "TitleThree",
                        Year = 2003,
                        EpisodeNumber = 1,
                        EpisodeOf_Id = 1,
                        Kind = "Kind",
                        SeasonNumber = 1,
                        SeriesYear = "2003"
                    }
                }
            };
        }
    }

    public class FakeDbSet<T> : IDbSet<T>
    where T : class
    {
        private ObservableCollection<T> _data;
        private IQueryable _query;

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
        public IDbSet<User> User { get; private set; }
        public IDbSet<Rating> Rating { get; private set; }
        public IDbSet<Person> People { get; private set; }
        public IDbSet<Participate> Participates { get; private set; }
        public IDbSet<PersonInfo> PersonInfoes { get; private set; }
        public IDbSet<InfoType> InfoTypes { get; private set; }

        public int SaveChanges()
        {
            return 0;
        }

        public void Dispose()
        {

        }
    }
}
