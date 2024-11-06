using Main.Domain.WorkflowDomain;
using Main.Domain.WorkflowDomain.Enum;

namespace main.DomainTest.Tests.WorkflowStepTests
{
    public class WorkflowStepConstructor
    {
        private class WorkflowStepCtor : WorkflowStep
        {
            public WorkflowStepCtor(
                Guid candidateId, 
                int number, 
                string? feedback, 
                string? lastFeedback, 
                string description, 
                Guid? employeeId, 
                Guid? lastEmployeeId, 
                Guid? roleId, 
                DateTime dateCreate, 
                DateTime dateUpdate,
                Status status, 
                DateTime? restartDate) 
                : base(
                      candidateId, 
                      number, 
                      feedback, 
                      lastFeedback, 
                      description, 
                      employeeId, 
                      lastEmployeeId, 
                      roleId, 
                      dateCreate, 
                      dateUpdate, 
                      status, 
                      restartDate)
            {}
        }

        private readonly IFixture _fixture;

        public WorkflowStepConstructor()
        {
            _fixture = new Fixture();
        }

        public static IEnumerable<object[]> GetInvalidData()
        {
            yield return new object[]
            {
                Guid.Empty, 1, "Feedback", "LastFeedback", "Description",
                Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow, Status.Expectation, null,
                typeof(ArgumentNullException), $"{Guid.Empty} - некорректный идентификатор кандидата"
            };
            yield return new object[]
            {
                Guid.NewGuid(), 0, "Feedback", "LastFeedback", "Description",
                Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow, Status.Expectation, null,
                typeof(ArgumentOutOfRangeException), "Некорректный номер шага процесса"
            };
            yield return new object[]
            {
                Guid.NewGuid(), 1, "Feedback", "LastFeedback", null,
                Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow, Status.Expectation, null,
                typeof(ArgumentNullException), "Описание шага процесса не может быть пустым"
            };
            yield return new object[]
            {
                Guid.NewGuid(), 1, "Feedback", "LastFeedback", "Description",
                null, Guid.NewGuid(), null, DateTime.UtcNow, DateTime.UtcNow, Status.Expectation, null,
                typeof(ArgumentNullException), "У шага должна быть привязка к конкретногому сотруднику или должности"
            };
            yield return new object[]
            {
                Guid.NewGuid(), 1, "Feedback", "LastFeedback", "Description",
                Guid.Empty, Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow, Status.Expectation, null,
                typeof(ArgumentNullException), $"{Guid.Empty} - некорректное значение для идентификатора сотрудника в шаге"
            };
            yield return new object[]
            {
                Guid.NewGuid(), 1, "Feedback", "LastFeedback", "Description",
                Guid.NewGuid(), Guid.NewGuid(), Guid.Empty, DateTime.UtcNow, DateTime.UtcNow, Status.Expectation, null,
                typeof(ArgumentNullException), $"{Guid.Empty} - некорректное значение для идентификатора должности в шаге"
            };
            yield return new object[]
            {
                Guid.NewGuid(), 1, "Feedback", "LastFeedback", "Description",
                Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), DateTime.MinValue, DateTime.UtcNow, Status.Expectation, null,
                typeof(ArgumentException), "Дата создания не может быть дефолтной."
            };
            yield return new object[]
            {
                Guid.NewGuid(), 1, "Feedback", "LastFeedback", "Description",
                Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow, DateTime.MinValue, Status.Expectation, null,
                typeof(ArgumentException), "Дата обновления не может быть дефолтной."
            };
        }

        [Fact]
        public void Success_Ctor_Create_With_Valid_Data()
        {
            var candidateId = _fixture.Create<Guid>();
            var number = 1;
            var feedback = "Feedback";
            var lastFeedback = "LastFeedback";
            var description = "Description";
            var employeeId = _fixture.Create<Guid>();
            var lastEmployeeId = _fixture.Create<Guid>();
            var roleId = _fixture.Create<Guid>();
            var dateCreate = DateTime.UtcNow;
            var dateUpdate = DateTime.UtcNow;
            var status = Status.Expectation;
            var restartAuthorEmployeeId = _fixture.Create<Guid>();
            var restartDate = DateTime.UtcNow;

            var step = new WorkflowStepCtor(
                candidateId,
                number,
                feedback,
                lastFeedback,
                description,
                employeeId,
                lastEmployeeId,
                roleId,
                dateCreate,
                dateUpdate,
                status,
                restartDate);

            Assert.NotNull(step);
            Assert.Equal(candidateId, step.CandidateId);
            Assert.Equal(number, step.Number);
            Assert.Equal(feedback, step.Feedback);
            Assert.Equal(lastFeedback, step.LastFeedback);
            Assert.Equal(description, step.Description);
            Assert.Equal(employeeId, step.EmployeeId);
            Assert.Equal(lastEmployeeId, step.LastEmployeeId);
            Assert.Equal(roleId, step.RoleId);
            Assert.Equal(dateCreate, step.DateCreate);
            Assert.Equal(dateUpdate, step.DateUpdate);
            Assert.Equal(status, step.Status);
            Assert.Equal(restartDate, step.RestartDate);
        }

        [Theory]
        [MemberData(nameof(GetInvalidData))]
        public void Failed_Ctor_Create_With_Invalid_Data(
            Guid candidateId,
            int number,
            string? feedback,
            string? lastFeedback,
            string description,
            Guid? employeeId,
            Guid? lastEmployeeId,
            Guid? roleId,
            DateTime dateCreate,
            DateTime dateUpdate,
            Status status,
            DateTime? restartDate,
            Type exceptionType,
            string exceptionMessage)
        {
            var exception = Assert.Throws(exceptionType, () =>
                new WorkflowStepCtor(
                    candidateId,
                    number,
                    feedback,
                    lastFeedback,
                    description,
                    employeeId,
                    lastEmployeeId,
                    roleId,
                    dateCreate,
                    dateUpdate,
                    status,
                    restartDate));

            Assert.NotNull(exception);
            Assert.Contains(exceptionMessage, exception.Message);
        }
    }
}
