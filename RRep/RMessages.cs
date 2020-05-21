using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using Microsoft.Reporting.WinForms;
namespace RRep
{
    public class RMessages : IReportViewerMessages
    {
        public string BackButtonToolTip { get { return ("Назад"); } }
        public string BackMenuItemText { get { return ("Назад"); } }
        public string ChangeCredentialsText { get { return ("Изменить"); } }
        public string CurrentPageTextBoxToolTip { get { return ("Текущая станица"); } }
        public string DocumentMapButtonToolTip { get { return ("Структура документа"); } }
        public string DocumentMapMenuItemText { get { return ("Структура"); } }
        public string ExportButtonToolTip { get { return ("Сохранить в файл"); } }
        public string ExportMenuItemText { get { return ("Сохранить"); } }
        public string FalseValueText { get { return ("Нет"); } }
        public string FindButtonText { get { return ("Поиск"); } }
        public string FindButtonToolTip { get { return ("Поиск по контекту"); } }
        public string FindNextButtonText { get { return ("Далее"); } }
        public string FindNextButtonToolTip { get { return ("Поиск далее"); } }
        public string FirstPageButtonToolTip { get { return ("Первая страница"); } }
        public string LastPageButtonToolTip { get { return ("Последняя страница"); } }
        public string NextPageButtonToolTip { get { return ("Следующая страница"); } }
        public string NoMoreMatches { get { return ("Не найдено"); } }
        public string NullCheckBoxText { get { return ("Пусто"); } }
        public string NullCheckBoxToolTip { get { return ("Пусто"); } }
        public string NullValueText { get { return ("Пусто"); } }
        public string PageOf { get { return ("Всего:"); } }
        public string PageSetupButtonToolTip { get { return ("Настрока страницы."); } }
        public string PageSetupMenuItemText { get { return ("Страница"); } }
        public string ParameterAreaButtonToolTip { get { return ("Область"); } }
        public string PasswordPrompt { get { return ("Ввод пароля."); } }
        public string PreviousPageButtonToolTip { get { return ("Предыдущая"); } }
        public string PrintButtonToolTip { get { return ("Печать"); } }
        public string PrintLayoutButtonToolTip { get { return ("Настройка печати"); } }
        public string PrintLayoutMenuItemText { get { return ("Настройка печати"); } }
        public string PrintMenuItemText { get { return ("Печать"); } }
        public string ProgressText { get { return ("Выполнение"); } }
        public string RefreshButtonToolTip { get { return ("Обновить"); } }
        public string RefreshMenuItemText { get { return ("Обновить"); } }
        public string SearchTextBoxToolTip { get { return ("Поиск"); } }
        public string SelectAValue { get { return ("Выбрать значение"); } }
        public string SelectAll { get { return ("Выбрать все"); } }
        public string StopButtonToolTip { get { return ("Остановить"); } }
        public string StopMenuItemText { get { return ("Стоп"); } }
        public string TextNotFound { get { return ("Текст не найден"); } }
        public string TotalPagesToolTip { get { return ("Все страницы"); } }
        public string TrueValueText { get { return ("Да"); } }
        public string UserNamePrompt { get { return ("Ввод пользователя"); } }
        public string ViewReportButtonText { get { return ("Отчет"); } }
        public string ViewReportButtonToolTip { get { return ("Просмотр отчета"); } }
        public string ZoomControlToolTip { get { return ("Масштаб"); } }
        public string ZoomMenuItemText { get { return ("Увеличить"); } }
        public string ZoomToPageWidth { get { return ("По ширине"); } }
        public string ZoomToWholePage { get { return ("На страницу"); } }
    }
}
