using System.Collections.Generic;

namespace Features.Clientes
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;
        //private readonly IMediator _mediator;

        public ClienteService(IClienteRepository clienteRepository
                                //,IMediator mediator
                                )
        {
            _clienteRepository = clienteRepository;
            //_mediator = _mediator;
        }

        public IEnumerable<Cliente> ObterTodosAtivos()
        {
            throw new System.NotImplementedException();
        }

        public void Adicionar(Cliente cliente)
        {
            if (!cliente.EhValido()) return;

            //_clienteRepository.Adicionar(cliente);
            //_mediator.Publish(new ClienteEmailNotification("admin@me.com", cliente.Email));
        }

        public void Atualizar(Cliente cliente)
        {
            if (!cliente.EhValido()) return;
            throw new System.NotImplementedException();
        }

        public void Remover(Cliente cliente)
        {
            throw new System.NotImplementedException();
        }

        public void Inativar(Cliente cliente)
        {
            if (!cliente.EhValido()) return;
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            //_clienteRepository.Dispose();
        }
    }
}