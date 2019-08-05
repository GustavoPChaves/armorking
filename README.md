# Armor King



## Uso do Repositório
### Configurando Git

```
git config --global user.name "Seu usuario"
git config --global user.email "seu email"
```

### Clone
Na pasta desejada, segure ```shift``` clique com ```botão direito do mouse``` e selecione abrir terminal aqui.

Clone o repositório na pasta desejada, copiando e colando esse comando no terminal:

```bash
git clone https://github.com/GustavoPChaves/armorking.git
```

## Pull
Você pode usar o comando git pull para automaticamente fazer o update do que estiver no GitHub para o seu computador:

```bash
git pull
```
## Criando Branches
Quando criamos uma branch, estamos fazendo uma ramificação no projeto, ou seja, criamos um outro projeto exatamente igual no qual faremos modificações desejadas.
```
git checkout -b "NomeDaBranch"
```
Nomeie a branch de acordo com o que irá realizar. Exemplo:
```
git checkout -b "PlayerAnimations"
```

## Add
Com o comando abaixo você estará adicionando as mudanças feitas no projeto ao Git:
```
git add .
```

## Commit
Para preparar as mudanças para o remoto utilize:
```
git commit -m "Mensagem"
```
Sempre escreva mensagens que descreva em uma frase o que está sendo adicionado ao repositório.

## Push
Enviando todas as modificações adicionadas e preparadas:
```
git push origin NomeDaBranch
```
## Pull Request
Após terminar sua tarefa, exemplo: todas as animações
 terminadas, Abra o GitHub e peça um Pull Request clicando em ```New Pull Request```
Escolha a branch que estava trabalhando e espere sua requisição ser avaliada.

O Avaliador pode aceitar suas modificações ou recusar. Caso recuse, será alertado por email o que deverá ser refeito para ser aceito.

## Atualizando a Master
Após suas modificações serem aceitas, o Administrador irá adiciona-las a ```Master``` 
Para ter o seu computador atualizado basta utilizar esses comandos:
```
git checkout Master
```
```
git checkout pull
```
Para continuar trabalhando e fazendo alterações volte para ```Criando Branches``` desse tutorial.

# Comandos Uteis

## Status
Ajuda a saber se precisa ```add```,```commit```ou ```push```em arquivos modificados.
```
git status
```

## Navegando pelas Branches
```
git checkout "NomeDaBranch"
```
## Listando Branches
```
git branch
```
## Remover Modificações feitas até o ultimo commit
```
git checkout .
```
