using Microsoft.Extensions.Options;
using RepositoryPatternUOW.Core.DTOs.Paymob.PaymobCardDto;
using System.Security.Cryptography;
using System.Text;

namespace Hospital.PaymobHmacService
{
    public class PaymobHmacService(IOptions<PaymobHmac> PaymobHmacOptions) : IPaymobHmacService
    {



        public string ComputeHmac(PaymobCardDto paymobCardDto)
        {
            string concatenatedString = GetConcatedRequestParams(paymobCardDto);

            using (var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(PaymobHmacOptions.Value.Hmac)))
            {
                var result = hmac.ComputeHash(Encoding.UTF8.GetBytes(concatenatedString));
                if (result is null) return string.Empty;
                return Convert.ToHexString(result).ToLowerInvariant();
            }
        }


        private string GetConcatedRequestParams(PaymobCardDto paymobDto)
        {
            return
                 paymobDto.obj.amount_cents.ToString() +
                 paymobDto.obj.created_at +
                 paymobDto.obj.currency +
                 paymobDto.obj.error_occured.ToString().ToLowerInvariant() +
                 paymobDto.obj.has_parent_transaction.ToString().ToLowerInvariant() +
                 paymobDto.obj.id.ToString() +
                 paymobDto.obj.integration_id.ToString() +
                 paymobDto.obj.is_3d_secure.ToString().ToLowerInvariant() +
                 paymobDto.obj.is_auth.ToString().ToLowerInvariant() +
                 paymobDto.obj.is_capture.ToString().ToLowerInvariant() +
                 paymobDto.obj.is_refunded.ToString().ToLowerInvariant() +
                 paymobDto.obj.is_standalone_payment.ToString().ToLowerInvariant() +
                 paymobDto.obj.is_voided.ToString().ToLowerInvariant() +
                 paymobDto.obj.order.id.ToString() +
                 paymobDto.obj.owner.ToString() +
                 paymobDto.obj.pending.ToString().ToLowerInvariant() +
                 paymobDto.obj.source_data.pan +
                 paymobDto.obj.source_data.sub_type +
                 paymobDto.obj.source_data.type +
                 paymobDto.obj.success.ToString().ToLowerInvariant()
                 ;
        }
    }
}
