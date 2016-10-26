<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="WebApplication10.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="Style.css" rel="stylesheet" />
    <script src="JavaScript.js"></script>
</head>
<body>

    <div class="cap">
        <h1>Тестовые задания</h1>
    </div>
    <form runat="server" action="default.aspx" method="post">
    <div class="data">

        <p>Сумма кредита <br/> Годовая ставка <br/> Срок погашения </p>  
        
        <asp:TextBox class="enter" ID="enterSumCredit" runat="server" OnKeyPress="EnsureNumeric()"></asp:TextBox>руб.   
        <asp:TextBox class="enter" ID="enterPercent" runat="server" OnKeyPress="EnsureNumeric()"></asp:TextBox>%
        <asp:TextBox class="enter" ID="enterMonth" runat="server" OnKeyPress="EnsureNumeric()"></asp:TextBox>мес.  
        
        <input class="btn"  type="submit" value="Построить график выплат" />

  
    </div>
    </form>
    <!-- первое тестовое задание-->
     <div class="TestOne">
         <div>
         <h1>Задание 1</h1>
        <asp:Label ID="Label1" runat="server" Text="Label">Исходный массив <br/> 1, 53, 4, 3, 41, 56, 45, 68, 15, 46, 64, 5, 6, 8, 51, 33, 54  </asp:Label>
        <br/>

        <%
            int[] mas = new int[] { 1, 53, 4, 3, 41, 56, 45, 68, 15, 46, 64, 5, 6, 8, 51, 33, 54 };
            
            for (int i = 0; i < mas.Length - 1; i++)
            {
                //поиск минимального числа
                int min = i;
                for (int j = i + 1; j < mas.Length; j++)
                {
                    if (mas[j] < mas[min])
                    {
                        min = j;
                    }
                }
                //обмен элементов
                int temp = mas[min];
                mas[min] = mas[i];
                mas[i] = temp;
            }
            
            Response.Write("<br/>Отсортированный массив:<br/>");
            for (int i = 0; i < mas.Length; i++)
            {
                Response.Write(mas[i] + " ");
            }     
        %>
             </div>
       </div>
</body>
</html>
