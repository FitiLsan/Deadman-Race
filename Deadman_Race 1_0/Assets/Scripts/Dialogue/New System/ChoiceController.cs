using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
/// <summary>
/// класс события изменения разговора
/// </summary>
//может ли он передать объект разговора, устанавливаем тип того,что передаем <Conversation>
//класс события изменения разговора
[System.Serializable]
public class ConversationChangeEvent : UnityEvent<Conversation> { }

public class ChoiceController : MonoBehaviour
{
    public Choice choice;
    //ссылка на событие смены диалога
    public ConversationChangeEvent conversationChangeEvent;
    /// <summary>
    /// создание кнопок в зависимости от кол-ва Choice в ScriptableObjecte Question
    /// </summary>
    /// <param name="choiceButtonTemplate"></param>
    /// <param name="choice"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public static ChoiceController AddChoiceButton (Button choiceButtonTemplate, Choice choice, int index)
    {
        //расстояние между кнопками
        int buttonSpacing = -44;
        //создаем кнопку
        Button button = Instantiate(choiceButtonTemplate);
        //создаем родительскую папку (контейнер)
        button.transform.SetParent(choiceButtonTemplate.transform.parent);
        //задаем размер кнопки
        button.transform.localScale = Vector3.one;
        //задаем положение кнопки (каждая создается ниже предыдущей кнопки в зависимости от значения Index)
        button.transform.localPosition = new Vector3(0, index * buttonSpacing, 0);
        //присваем имя объекту (первая кнопка будет названа Choice 1)
        button.name = "Choice" + (index + 1);
        //делаем активным на сцене (так как объект Question  и его потомки установлены "не активными")
        button.gameObject.SetActive(true);
        //получаем у кнопки компонент <ChoiceController>
        ChoiceController choiceController = button.GetComponent<ChoiceController>();
        //кнопка выбора будет иметь возможность делать выбор среди сохраннеых вариантов
        choiceController.choice = choice;
        return choiceController;
    }
    private void Start()
    {
        //если нет события изменения разговора
        if (conversationChangeEvent == null)
        {
            //делаем новое событие изменения разговора
            conversationChangeEvent = new ConversationChangeEvent();
        }
        //передаем значение в кнопку из дочернего компонента кнопки Текст - текст в кнопку
        GetComponent<Button>().GetComponentInChildren<Text>().text = choice.text;
    }
    //вешается на кнопку Choice Button в компонент Button - On Click
    public void MakeChoice()
    {
        conversationChangeEvent.Invoke(choice.conversation);
    }
}
