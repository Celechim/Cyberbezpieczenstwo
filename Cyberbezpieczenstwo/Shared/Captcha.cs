using System;
namespace Cyberbezpieczenstwo.Shared;

public class Captcha
{
    public int Id { get; set; }
    public String Question { get; set; }
    public String Answer { get; set; }

    public Captcha(int id, string q, string a)
    {
        Id = id;
        Question = q;
        Answer = a;
    }

    public static Captcha GenerateCaptchaQuestion()
    {
        var questions = new List<Captcha>()
    {
        new Captcha
        (
            1,
            "Stolica Wielkiej Brytanii to...?",
            "Londyn"
        ),
        new Captcha
        (
            2,
            "Symbol chemiczny wodoru to...?",
            "H"
        ),
        new Captcha
        (
            3,
            "Kolumb odkrył Amerykę w roku...?",
            "1492"
        ),
        new Captcha
        (
            4,
            "Państwo z największą liczbą ludności to...?",
            "Chiny"
        ),
        new Captcha
        (
            5,
            "Jedyny naturalny satelita Ziemi nosi nazwę...?",
            "Księżyc"
        ),
        new Captcha
        (
            6,
            "Najlepszy przyjaciel człowieka to...?",
            "Pies"
        ),
        new Captcha
        (
            7,
            "Najwyższy na świecie szczyt górski nosi nazwę...?",
            "Mount Everest"
        ),
        new Captcha
        (
            8,
            "Grecki bóg wojny nosił imię...?",
            "Ares"
        ),
        new Captcha
        (
            9,
            "Jakie zwierzę zyskało w kulturze miano króla dżungli...?",
            "Lew"
        ),
        new Captcha
        (
            10,
            "Jak nazywa się najmroźniejszy kontynent świata...?",
            "Antarktyda"
        ),
    };

        var rand = new Random();
        return questions.ToArray()[rand.Next(1, 10)];
    }

}