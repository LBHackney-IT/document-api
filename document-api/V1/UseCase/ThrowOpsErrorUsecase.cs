
namespace document_api.UseCase.V1
{
    public class ThrowOpsErrorUsecase
    {
        public static void  Execute()
        {
            throw new TestOpsErrorException();
        }
    }
}
