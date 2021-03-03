namespace DotnetCoreInfra.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Id is Require
    /// </summary>
    /// <seealso cref="System.Exception" />
    [Serializable]
    public class RequireIdException : Exception
    {
        /// <summary>The entity type</summary>
        private readonly Type type;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequireIdException"/> class.
        /// </summary>
        public RequireIdException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequireIdException"/> class.
        /// </summary>
        /// <param name="type">The entity type.</param>
        public RequireIdException(Type type)
        {
            this.type = type;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequireIdException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public RequireIdException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequireIdException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public RequireIdException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequireIdException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination.</param>
        protected RequireIdException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>Gets a message that describes the current exception.</summary>
        public override string Message => $"缺少Id键值信息,无法插入{type}类型的实体数据";
    }
}
