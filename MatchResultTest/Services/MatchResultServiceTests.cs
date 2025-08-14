using NUnit.Framework;
using NSubstitute;
using MatchResult.Services;
using MatchResult.Repositories;
using MatchResult.Models;

namespace MatchResultTest.Services
{
    [TestFixture]
    public class MatchResultServiceTests
    {
        private IMatchResultRepository _mockRepository;
        private IMatchResultService _service;

        [SetUp]
        public void SetUp()
        {
            _mockRepository = Substitute.For<IMatchResultRepository>();
            _service = new MatchResultService(_mockRepository);
        }

        [Test]
        public void UpdateMatchResult_ShouldAddHomeGoal_WhenEventIsHomeGoal()
        {
            // Arrange
            int matchId = 1;
            _mockRepository.GetRawData(matchId).Returns("HA");

            // Act
            var result = _service.UpdateMatchResult(matchId, Event.HomeGoal);

            // Assert
            Assert.AreEqual("HAH", result);
        }

        [Test]
        public void UpdateMatchResult_ShouldAddPeriod_WhenEventIsPeriod()
        {
            // Arrange
            int matchId = 1;
            _mockRepository.GetRawData(matchId).Returns("HA");

            // Act
            var result = _service.UpdateMatchResult(matchId, Event.Period);

            // Assert
            Assert.AreEqual("HA;", result);
        }

        [Test]
        public void UpdateMatchResult_ShouldRemoveLastHomeGoal_WhenEventIsHomeCancel()
        {
            // Arrange
            int matchId = 2;
            _mockRepository.GetRawData(matchId).Returns("HAH");

            // Act
            var result = _service.UpdateMatchResult(matchId, Event.HomeCancel);

            // Assert
            Assert.AreEqual("HA", result);
        }

        [Test]
        public void UpdateMatchResult_ShouldThrowException_WhenEventIsAwayCancelAndNoAwayGoal()
        {
            // Arrange
            int matchId = 2;
            _mockRepository.GetRawData(matchId).Returns("HAH");

            // Act & Assert
            var ex = Assert.Throws<UpdateMatchResultException>(() => _service.UpdateMatchResult(matchId, Event.AwayCancel));
            Assert.That(ex.Message, Is.EqualTo("Invalid operation: AwayCancel on HAH"));
        }

        [Test]
        public void UpdateMatchResult_ShouldRemoveLastAwayGoal_WhenEventIsAwayCancel()
        {
            // Arrange
            int matchId = 3;
            _mockRepository.GetRawData(matchId).Returns("HA;");

            // Act
            var result = _service.UpdateMatchResult(matchId, Event.AwayCancel);

            // Assert
            Assert.AreEqual("H;", result);
        }
    }
} 