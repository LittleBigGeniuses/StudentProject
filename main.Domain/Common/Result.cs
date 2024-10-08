﻿namespace Main.Domain.Common
{

    /// <summary>
    /// Примитивная реализация паттерна Result
    /// </summary>
    /// <typeparam name="T">Ожидаемый тип сущности к возвращению</typeparam>
    public class Result<T>
    {
        /// <summary>
        /// Возвращаемый объект
        /// </summary>
        public T? Value { get; set; }

        /// <summary>
        /// Сообщение о провале
        /// </summary>
        public string? Error { get; set; }

        /// <summary>
        /// Успешность выполнения
        /// </summary>
        public bool IsSuccess { get; private set; } = true;

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
            var result = new Result<T>();
            result.IsSuccess = false;
            result.Error = error;

            return result;
        }

        /// <summary>
        /// Метод успешного создания объекта
        /// </summary>
        /// <param name="data">Объект</param>
        /// <returns>Возвращает сущность содержащую ожидаемый объект</returns>
        public static Result<T> Success(T data)
        {
            var result = new Result<T>();

            result.IsSuccess = true;
            result.Value = data;

            return result;
        }
    }
}
