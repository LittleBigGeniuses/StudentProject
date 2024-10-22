namespace Main.Domain.Common
{

    /// <summary>
    /// Паттерна Result
    /// </summary>
    /// <typeparam name="T">Ожидаемый тип сущности к возвращению</typeparam>
    public class Result<T>
    {
        /// <summary>
        /// Приватный конструктор
        /// </summary>
        private Result(T? value, string? error, bool isSuccess)
        {
            Value = value;
            Error = error;
            IsSuccess = isSuccess;
        }

        /// <summary>
        /// Возвращаемый объект
        /// </summary>
        public T? Value { get; }

        /// <summary>
        /// Сообщение о провале
        /// </summary>
        public string? Error { get; }

        /// <summary>
        /// Успешность выполнения
        /// </summary>
        public bool IsSuccess { get;} 

        /// <summary>
        /// Неуспешность выполнения
        /// </summary>
        public bool IsFailure => !IsSuccess;

        /// <summary>
        /// Метод сообщения о провале создания объекта
        /// </summary>
        /// <param name="error">Сообщение провала</param>
        /// <returns>Возвращает сущность с сообщение о провале</returns>
        public static Result<T> Failure(string error)
        {
            if (string.IsNullOrEmpty(error))
                throw new ArgumentException("Failure result must have a non-empty error message", nameof(error));

            return new Result<T>(default, error, false);
        }

        /// <summary>
        /// Метод успешного создания объекта
        /// </summary>
        /// <param name="data">Объект</param>
        /// <returns>Возвращает сущность содержащую ожидаемый объект</returns>
        public static Result<T> Success(T data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data), "Success result cannot have a null value");

            return new Result<T>(data, null, true);
        }
    } 
}
