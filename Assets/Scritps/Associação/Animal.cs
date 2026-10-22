using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal
{
    public string Nome { get; set; }
    public string ImagemUrl { get; set; }
    public Sprite Imagem { get; set; }

    public Animal()
    {
        Nome = "";
        ImagemUrl = "";
        Imagem = null;
    }

    public Animal(string nome, string imagemUrl)
    {
        Nome = nome;
        ImagemUrl = imagemUrl;
        Imagem = null;
    }

    public Animal(string nome, Sprite imagem)
    {
        Nome = nome;
        Imagem = imagem;
        ImagemUrl = "";
    }
}
