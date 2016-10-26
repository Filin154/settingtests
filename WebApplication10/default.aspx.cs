using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication10
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        /*
         month - срок кредитования (в месяцах)
         monthPay - месячный платеж
         credit - сумма кредита
         yearRate - годовая ставка (в процентах)
         montPercent - проценты по кредиту за месяц
         payDolg - сумма которая идет на основного долга

        totalPayDolg - общая сумма средств идущая на погашение основного долга
        totalMontPercent общая сумма ежемесячных процентов
        totalmonthPay - общая сумма выплат за весь период
        totalOverPay - общая сумма выплат сверх ежемесячных платежей
         */
        int month;
        double monthPay, credit, yearRate, montPercent, payDolg;
        double totalPayDolg, totalMontPercent, totalmonthPay, totalOverPay;
        protected void Page_Load(object sender, EventArgs e)
        {
            /*
             если страница загружается впервые то присваиаем полям значения по умолчанию
             */
            if (!IsPostBack)
            {
                enterSumCredit.Text = "0";
                enterPercent.Text = "0";
                enterMonth.Text = "0";
            }

            /*
             если страница загружается не в первый раз и при этом не заполнены какие то поля
             то заполняем поля значенями по умолчанию. так же рисуем шапку таблицы
             */
            if (IsPostBack)
            {
                if (enterSumCredit.Text.Length == 0 || enterPercent.Text.Length == 0 || enterMonth.Text.Length == 0)
                {
                    enterSumCredit.Text = "0";
                    enterPercent.Text = "0";
                    enterMonth.Text = "0";
                }

                Response.Write(
                    "<table class=\"header\">" +
                      "<tr>" +
                        "<td>" + "Порядковый номер месяца" + "</td>" +
                        "<td>" + "Платеж в счет погашения основного долга, руб" + "</td>" +
                        "<td>" + "Проценты по кредиту, руб" + "</td>" +
                        "<td>" + "Общий ежемесячный платеж, руб" + "</td>" +
                        "<td>" + "Остаток осн. долга после совершения текущего платежа, руб" + "</td>" +
                        "<td>" + "Досрочное погашение (сверх ежемесячного платежа)" + "</td>" +
                      "</tr>" +
                    "</table> ");
            }

            /*
              считываем значения из заполненных полей
             */
            credit = Convert.ToInt32(enterSumCredit.Text);
            yearRate = Convert.ToDouble(enterPercent.Text);
            month = Convert.ToInt32(enterMonth.Text);

            /*
              yearRate введено в процентах, конвертируем в десятичный вид
             */
            yearRate = yearRate / 100;
            yearRate = yearRate / 12;

            /*
             считаем месячный платеж
            */
            monthPay = credit * (yearRate + (yearRate / (Math.Pow((1 + yearRate), month) - 1)));
           
        }

        /*
        При наступлении события preRender рисуем таблицу
       */
        protected void Page_preRender(object sender, EventArgs e)
        {
            Response.Write("<form action=\"default.aspx\" method=\"post\">");
                       
            /*
             Высчитываем все значения и рисуем таблицу
             */
            for (int i = 1; i <= month; i++)
            {
                /*
                cчитываем дополнительную сумму для досрочного погашения долга
                */
                string param = Request.Form[Convert.ToString(i)];

                /*
                если дополнительная сумма превышает сумму долга, досрочно завершаем цикл со значение переплаты
                */
                if (credit < Convert.ToDouble(param))
                {
                    montPercent = credit * yearRate;
                    payDolg = monthPay - montPercent;
                    credit = credit - payDolg;
                  
                    credit -= Convert.ToDouble(param);

                    Response.Write(
                        "<table>" +
                           "<tr>" +
                             "<td>" + (i) + " мес." + "</td>" +
                             "<td>" + Math.Round(payDolg, 2) + "</td>" +
                             "<td>" + Math.Round(montPercent, 2) + "</td>" +
                             "<td>" + Math.Round(monthPay, 2) + "</td>" +
                             "<td>" + Math.Round(credit, 2) + "</td>" +
                             "<td>" + "<input name=\"" + i + "\" id=\"Text1\" type=\"text\" value=\"" + Convert.ToDouble(param) + "\" OnKeyPress=\"EnsureNumeric()\" />  <br/>" + "</td>" +
                           "</tr>" +
                         "</table>");

                    totalPayDolg += payDolg;
                    totalMontPercent += montPercent;
                    totalmonthPay += monthPay;
                    totalOverPay += Convert.ToDouble(param);

                    Response.Write(
                               "<table class=\"header\" >" +
                                 "<tr>" +
                                   "<td>" + " Всего" + "</td>" +
                                   "<td>" + Math.Round(totalPayDolg, 2) + "</td>" +
                                   "<td>" + Math.Round(totalMontPercent, 2) + "</td>" +
                                   "<td>" + Math.Round(totalmonthPay, 2) + "</td>" +
                                   "<td>" + " " + "</td>" +
                                   "<td>" + totalOverPay + "</td>" +
                                 "</tr>" +
                                 "<tr>" +
                                      "<td colspan=\"6\">" + "<br/> " + "</td>" +
                                 "</tr>" +
                                 "<tr>" +
                                      "<td colspan=\"4\">" + "Общая сумма выплат с учетом досрочного погашения:" + "</td>" +
                                      "<td colspan=\"4\">" + Math.Round(totalmonthPay, 2) + "</td>" +
                                 "</tr>" +
                               "</table>");

                    break;
                }
                
                /*
                если дополнительная сумма для погашения долга не введена либо меньше задолжности
                то продолжаем цикл, выводим значения в таблицу
                */
                credit -= Convert.ToDouble(param);

                montPercent = credit * yearRate;
                payDolg = monthPay - montPercent;
                credit = credit - payDolg;

                Response.Write(
                        "<table>" +
                           "<tr>" +
                             "<td>" + (i) + " мес." + "</td>" +
                             "<td>" + Math.Round(payDolg, 2) + "</td>" +
                             "<td>" + Math.Round(montPercent, 2) + "</td>" +
                             "<td>" + Math.Round(monthPay, 2) + "</td>" +
                             "<td>" + Math.Round(credit, 2) + "</td>" +
                             "<td>" + "<input name=\"" + i + "\" id=\"Text1\" type=\"text\" value=\"" + Convert.ToDouble(param) + "\" OnKeyPress=\"EnsureNumeric()\" />  <br/>" + "</td>" +
                           "</tr>" +
                         "</table>");
               

                totalPayDolg += payDolg;
                totalMontPercent += montPercent;
                totalmonthPay += monthPay;
                totalOverPay += Convert.ToDouble(param);
                
                /*
               на каждой иттерации высчитываем новую самму месячного платежа
               */
                monthPay = credit * (yearRate + (yearRate / (Math.Pow((1 + yearRate), (month - i)) - 1)));

                /*
                в конце таблицы выводим Общие показатели за весь период
                */
                if (i == month)
                {
                    Response.Write(
                               "<table class=\"header\" >" +
                                 "<tr>" +
                                   "<td>" + " Всего" + "</td>" +
                                   "<td>" + Math.Round(totalPayDolg, 2) + "</td>" +
                                   "<td>" + Math.Round(totalMontPercent, 2) + "</td>" +
                                   "<td>" + Math.Round(totalmonthPay, 2) + "</td>" +
                                   "<td>" + " " + "</td>" +
                                   "<td>" + totalOverPay + "</td>" +                                   
                                 "</tr>" +
                                 "<tr>"+
                                      "<td colspan=\"6\">"+"<br/> "+"</td>" +
                                 "</tr>"+
                                 "<tr>" +
                                      "<td colspan=\"4\">" + "Общая сумма выплат с учетом досрочного погашения:" + "</td>" +
                                      "<td colspan=\"4\">" + Math.Round(totalmonthPay, 2) + "</td>" +
                                 "</tr>" +
                               "</table>");
                }
            }
            
            Response.Write("</ form >");
        }
    }
}
