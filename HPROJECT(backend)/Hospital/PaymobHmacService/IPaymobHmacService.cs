using RepositoryPatternUOW.Core.DTOs.Paymob.PaymobCardDto;
namespace Hospital.PaymobHmacService
{
    public interface IPaymobHmacService
    {

        public string ComputeHmac(PaymobCardDto paymobCardDto);

    }
}
