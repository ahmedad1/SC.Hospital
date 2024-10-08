namespace RepositoryPatternWithUOW.EfCore.InitPayService
{
    public interface IInitPaymentService
    {
        public Task<string?> InitPay(string firstName, string lastName, string email, int doctorId, string pDescription,int patientId);

    }
}
