document.addEventListener('DOMContentLoaded', () => {
    const apiBaseUrl = '/api/contatos';
    const tabelaContatosBody = document.getElementById('tabelaContatosBody');
    const formBusca = document.getElementById('formBusca');
    const inputBusca = document.getElementById('inputBusca');
    const btnNovoContato = document.getElementById('btnNovoContato');

    const contatoModalElement = document.getElementById('contatoModal');
    const contatoModal = new bootstrap.Modal(contatoModalElement);
    const modalTitulo = document.getElementById('modalTitulo');
    const formContato = document.getElementById('formContato');
    const emailsContainer = document.getElementById('emailsContainer');
    const btnAdicionarEmail = document.getElementById('btnAdicionarEmail');

    const carregarContatos = async (filtro = '') => {
        const url = filtro ? `${apiBaseUrl}?filtro=${encodeURIComponent(filtro)}` : apiBaseUrl;
        try {
            const response = await fetch(url);
            if (!response.ok) throw new Error('Erro ao buscar contatos');
            const contatos = await response.json();

            tabelaContatosBody.innerHTML = ''; // Limpa a tabela
            contatos.forEach(contato => {
                const tr = document.createElement('tr');
                tr.innerHTML = `
                    <td>${contato.nome}</td>
                    <td>${contato.empresa || ''}</td>
                    <td>
                        ${[contato.telefonePessoal, contato.telefoneComercial].filter(Boolean).join(' / ')}
                    </td>
                    <td class="text-end">
                        <button class="btn btn-sm btn-warning btn-editar" data-id="${contato.id}">Editar</button>
                        <button class="btn btn-sm btn-danger btn-excluir" data-id="${contato.id}">Excluir</button>
                    </td>
                `;
                tabelaContatosBody.appendChild(tr);
            });
        } catch (error) {
            console.error(error);
            tabelaContatosBody.innerHTML = `<tr><td colspan="4" class="text-center text-danger">${error.message}</td></tr>`;
        }
    };

    const adicionarCampoEmail = (email = '') => {
        const div = document.createElement('div');
        div.className = 'email-input-group';
        div.innerHTML = `
            <input type="email" class="form-control" value="${email}" placeholder="email@exemplo.com">
            <button type="button" class="btn btn-danger btn-sm btn-remover-email">Remover</button>
        `;
        emailsContainer.appendChild(div);
    };

    btnNovoContato.addEventListener('click', () => {
        modalTitulo.textContent = 'Novo Contato';
        formContato.reset();
        document.getElementById('contatoId').value = '';
        emailsContainer.innerHTML = '';
        adicionarCampoEmail(); 
        contatoModal.show();
    });

    btnAdicionarEmail.addEventListener('click', () => adicionarCampoEmail());

    emailsContainer.addEventListener('click', (e) => {
        if (e.target.classList.contains('btn-remover-email')) {
            e.target.closest('.email-input-group').remove();
        }
    });

    formContato.addEventListener('submit', async (e) => {
        e.preventDefault();

        const id = document.getElementById('contatoId').value;
        const emails = [...emailsContainer.querySelectorAll('input[type="email"]')]
            .map(input => input.value)
            .filter(email => email.trim() !== ''); 

        const contato = {
            id: id ? parseInt(id) : 0,
            nome: document.getElementById('nome').value,
            empresa: document.getElementById('empresa').value,
            telefonePessoal: document.getElementById('telPessoal').value,
            telefoneComercial: document.getElementById('telComercial').value,
            emails: emails
        };

        const method = id ? 'PUT' : 'POST';
        const url = id ? `${apiBaseUrl}/${id}` : apiBaseUrl;

        try {
            const response = await fetch(url, {
                method: method,
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(contato)
            });

            if (!response.ok) {
                throw new Error(`Erro ao salvar: ${response.statusText}`);
            }

            contatoModal.hide();
            carregarContatos();
        } catch (error) {
            console.error(error);
            alert(error.message);
        }
    });

    tabelaContatosBody.addEventListener('click', async (e) => {
        const id = e.target.dataset.id;
        if (!id) return;

        if (e.target.classList.contains('btn-editar')) {
            try {
                const response = await fetch(`${apiBaseUrl}/${id}`);
                const contato = await response.json();

                modalTitulo.textContent = 'Editar Contato';
                formContato.reset();
                emailsContainer.innerHTML = '';

                document.getElementById('contatoId').value = contato.id;
                document.getElementById('nome').value = contato.nome;
                document.getElementById('empresa').value = contato.empresa || '';
                document.getElementById('telPessoal').value = contato.telefonePessoal || '';
                document.getElementById('telComercial').value = contato.telefoneComercial || '';

                contato.emails.forEach(email => adicionarCampoEmail(email));
                if (contato.emails.length === 0) adicionarCampoEmail();

                contatoModal.show();
            } catch (error) {
                console.error(error);
            }
        }

        if (e.target.classList.contains('btn-excluir')) {
            if (confirm('Tem certeza que deseja excluir este contato?')) {
                try {
                    await fetch(`${apiBaseUrl}/${id}`, { method: 'DELETE' });
                    carregarContatos();
                } catch (error) {
                    console.error(error);
                }
            }
        }
    });

    let timeoutId = null;
    inputBusca.addEventListener('keyup', () => {
        clearTimeout(timeoutId);
        timeoutId = setTimeout(() => {
            carregarContatos(inputBusca.value);
        }, 300); 
    });

    carregarContatos();
});