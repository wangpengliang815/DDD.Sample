namespace Ordering.Domain.Commands
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using MediatR;

    using Ordering.Domain.AggregateModels;
    using Ordering.Domain.Validations;

    public class OrderAddCommandArgs : OrderCommandArgs
        , IRequest<object>
    {
        public OrderAddCommandArgs(Guid id, decimal totalPrice, List<OrderItem> orderItems)
        {
            Id = id;
            TotalPrice = totalPrice;
            OrderItems = orderItems;
        }

        public override bool IsValid()
        {
            ValidationResult = new OrderAddCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
