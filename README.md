# Ninja Frog Adventure

**Ninja Frog Adventure** é um jogo de plataforma 2D desenvolvido na **Unity 6** como projeto da disciplina de **Game Development**. O jogo apresenta uma aventura em fases, com progressão de dificuldade, coleta de moedas, inimigos, sistema de vidas, respawn, portais entre fases, menu inicial e tela de vitória.

## Sobre o jogo

O jogador controla um sapo ninja em uma aventura de plataforma 2D. O objetivo é atravessar as fases, coletar moedas, evitar ou derrotar inimigos e chegar ao portal final para avançar até a tela de vitória.

O projeto foi desenvolvido com foco em mecânicas simples, responsivas e funcionais, utilizando recursos da Unity para física 2D, animações, colisões, interface, áudio e troca de cenas.

## Mecânicas implementadas

* Movimento lateral do personagem
* Corrida
* Pulo
* Pulo duplo
* Wall jump / salto em parede
* Sistema de vidas
* Dano ao encostar em inimigos
* Respawn ao perder todas as vidas ou cair da fase
* Coleta de moedas
* HUD com vidas e moedas
* Inimigos com patrulha
* Inimigos derrotáveis ao pular em cima
* Efeitos sonoros de ações principais
* Portais para troca de fase
* Menu inicial
* Tela final de vitória

## Controles

| Ação                | Tecla                           |
| ------------------- | ------------------------------- |
| Andar para esquerda | A ou Seta Esquerda              |
| Andar para direita  | D ou Seta Direita               |
| Correr              | Shift                           |
| Pular               | Espaço, W ou Seta para Cima     |
| Pulo duplo          | Pressionar pulo novamente no ar |
| Wall jump           | Pular próximo a uma parede      |

## Fases

O jogo possui 3 fases com dificuldade progressiva:

### Level_01

Fase inicial com foco em movimentação básica, coleta de moedas e introdução aos inimigos.

### Level_02

Fase com plataformas mais afastadas, inimigo diferente e maior exigência de controle do pulo.

### Level_03

Fase final com mais desafios, obstáculos e portal que leva à tela de vitória.

## Interface e HUD

O jogo possui uma interface simples com:

* Corações representando a vida do jogador
* Contador de moedas coletadas
* Menu inicial com botões de jogar e sair
* Tela de vitória com opções de jogar novamente, voltar ao menu ou sair

## Áudio

O projeto utiliza áudio para melhorar o feedback do jogador, incluindo:

* Música de fundo
* Som de pulo
* Som de dano
* Som de coleta de moeda
* Som de inimigo
* Som de portal/finalização

## Tecnologias utilizadas

* Unity 6
* C#
* Unity 2D Physics
* Unity Animator
* TextMeshPro
* Pixel Perfect Camera
* Tilemap 2D

## Como executar o projeto na Unity

1. Baixe ou clone este repositório.
2. Abra o projeto pela Unity Hub.
3. Use a versão **Unity 6** ou compatível.
4. Abra a cena `MainMenu`.
5. Clique em **Play** para iniciar o jogo.

## Como jogar a build exportada

1. Baixe o arquivo compactado da build.
2. Extraia a pasta.
3. Abra o arquivo executável do jogo.
4. Não remova os arquivos da pasta, pois o executável depende deles para funcionar.

## Cenas do jogo

```text
MainMenu
Level_01
Level_02
Level_03
Victory
```

A ordem correta no Build Profiles é:

```text
0 - MainMenu
1 - Level_01
2 - Level_02
3 - Level_03
4 - Victory
```

## Créditos de assets

Este projeto utiliza sprites, sons e recursos visuais gratuitos para fins educacionais.

```text
Sprites:
- https://pixelfrog-assets.itch.io/pixel-adventure-1
- https://essssam.itch.io/rocky-roads

Áudios:
- https://pixabay.com/pt/music/jogos-de-v%C3%ADdeo-cruising-down-8bit-lane-159615
- https://brackeysgames.itch.io/brackeys-platformer-bundle

```

## Autor

Desenvolvido por **João Gabriel** para a disciplina de **Game Development**.

## Status do projeto

Projeto desenvolvido como trabalho acadêmico, contendo uma experiência jogável completa com início, fases, desafios, inimigos, HUD, áudio e tela final.
