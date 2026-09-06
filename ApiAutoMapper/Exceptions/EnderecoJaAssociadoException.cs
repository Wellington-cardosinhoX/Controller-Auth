namespace ApiAutoMapper.Exceptions
{
    public class EnderecoJaAssociadoException : Exception
    {
        public EnderecoJaAssociadoException() : base("Endereço já associado a um cinema") { }
    }
}
