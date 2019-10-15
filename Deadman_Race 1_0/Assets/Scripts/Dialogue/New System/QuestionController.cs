using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// делает возможным появление вопроса в диалоге
/// </summary>
public class QuestionController : MonoBehaviour
{
    //вопрос появляется самостоятельно, когда диалог закончится(все персонажи скажут свои строки), если у скриптаблОбжекта Conversation (разговор) есть ссылка на Question
    public Question question;
    //ссылка на окно UI Text с расположением текста
    public Text questionText;
    //ссылка на префаб образца кнопки (и как распологается кнопка в UI). Нужен для создания аналогичных кнопок по оформлению с помощью ChoiceController
    public Button choiceButton;
    //список из кнопок с выбором действий
    private List<ChoiceController> choiceControllers = new List<ChoiceController>();
    
    public void Change(Question _question)
    {
        RemoveChoices();
        //вопрос из скриптаблОбжект Question передается в метод
        question = _question;
        //активирует UI у вопроса Question
        gameObject.SetActive(true);
        Initialize();
    }
    /// <summary>
    /// скрывает все разговоры
    /// </summary>
    /// <param name="conversation"></param>
    public void Hide(Conversation conversation)
    {
        RemoveChoices();
        //деактивирует UI у вопроса Question
        gameObject.SetActive(false);
    }
    /// <summary>
    /// удалить кнопки выбора "choice"
    /// </summary>
    private void RemoveChoices()
    {
        //пробегается по добавленным на сцене кнопкам и удаляет их
        foreach (ChoiceController c in choiceControllers)
            Destroy(c.gameObject);
        //очищает список из кнопок выбора - Сhoice
        choiceControllers.Clear();
    }
    private void Start()
    {
        
    }
    private void Initialize()
    {
        //текст из скриптаблОбжект Question передается в окно UI текст
        questionText.text = question.text;
        //пробегаемся по всем choices из скриптаблОбжект Question
        for (int index = 0; index < question.choices.Length; index++)
        {
            //добавляем кнопки в соответствии с индексом choice из скриптаблОбжект Question в список
            ChoiceController c = ChoiceController.AddChoiceButton(choiceButton, question.choices[index], index);
            choiceControllers.Add(c);
        }
        //образец кнопки(префаб) диактивируем
        choiceButton.gameObject.SetActive(false);
    }
}
