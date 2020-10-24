Funcionalidade: Pedido - Adicionar Item ao Carrinho
	Como um usuário
	Eu desejo colocar im item no carrinho
	Para que eu possa comprá-lo posteriormente

Cenário: Adicionar item com sucesso a um novo pedido
Dado O usuário esteja logado
E Que um produto esteja na vitrine
E Esteja disponível no estoque
E Não tenha nenhum produto adicionado ao carrinho
Quando O usuário adicionar uma unidade ao carrinho
Então O usuário será redireciondo ao resumo da compra
E O valor total do pedido será exatamente o valor do item adicionado

Cenário: Adicionar items acima do limite
Dado O usuário esteja logado
E Que um produto esteja na vitrine
E Esteja disponível no estoque
Quando O usuário adicionar uma item acima da quantidade máxima permitida
Então Receberá uma mensagem de erro mencionando que foi ultrapassada a quantidade limite

Cenário: Adicionar item já existente no carrinho
Dado O usuário esteja logado
E Que um produto esteja na vitrine
E Esteja disponível no estoque
E O mesmo produto já tenha sido adicionado ao carrinho anteriormente
Quando O usuário adicionar uma unidade ao carrinho
Então O usuário será redireciondo ao resumo da compra
E A quantidade de itens daquele produto terá sido acrescida em uma unidade a mais
E O valor total do pedido será a multiplicação da quantidade de itens pelo valor unitário

Cenário: Adicionar item já existente onde soma ultrapassa limite máximo
Dado O usuário esteja logado
E Que um produto esteja na vitrine
E Esteja disponível no estoque
E O mesmo produto já tenha sido adicionado ao carrinho anteriormente
Quando O usuário adicionar a quantidade máxima permitida ao carrinho
Então Receberá uma mensagem de erro mencionando que foi ultrapassada a quantidade limite
