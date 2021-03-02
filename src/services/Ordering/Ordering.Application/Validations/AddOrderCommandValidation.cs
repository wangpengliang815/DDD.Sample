using Ordering.Application.Commands;

namespace Ordering.Application.Validations
{
    public class AddOrderCommandValidation : OrderValidation<OrderCommandArgs>
    {
        public AddOrderCommandValidation()
        {
            ValidateName();//验证收货姓名
            ValidatePhone();//验证收货人手机号
            //可以自定义增加新的验证
        }
    }
}
