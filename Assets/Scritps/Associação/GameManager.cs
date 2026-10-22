using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private List<Animal> animais = new List<Animal>();
    private List<string> animaisSelecionados = new List<string>();
    public delegate void AnimalSelecionadoHandler(Animal animal);
    public event AnimalSelecionadoHandler OnAnimalSelecionado;

    public void ReceberAnimal(Animal animal)
    {
        if (animal != null)
        {
            if (!AnimalJaSelecionado(animal.Nome))
            {
                animais.Add(animal);
                AnimalSelecionado(animal.Nome);
                OnAnimalSelecionado?.Invoke(animal);
                Debug.Log("Animal adicionado: " + animal.Nome);
            }
            else
            {
                Debug.LogWarning("Animal já selecionado: " + animal.Nome);
            }
        }
        else
        {
            Debug.LogWarning("Nenhum animal foi recebido.");
        }
    }

    public Animal GetAnimalByName(string animalName)
    {
        string nomeNormalizado = animalName.Trim().ToLower();

        foreach (Animal animal in animais)
        {
            if (animal.Nome.ToLower() == nomeNormalizado)
            {
                Debug.Log("Animal encontrado: " + animal.Nome);
                return animal;
            }
        }

        Debug.LogWarning("Animal não encontrado: " + animalName);
        return null;
    }

    public List<Animal> GetAllAnimals()
    {
        return new List<Animal>(animais);
    }

    public bool AnimalJaSelecionado(string nome)
    {
        return animaisSelecionados.Contains(nome.Trim().ToLower());
    }

    public void AnimalSelecionado(string nome)
    {
        animaisSelecionados.Add(nome.Trim().ToLower());
    }

    public void ResetarAnimaisSelecionados()
    {
        animaisSelecionados.Clear();
    }
}
