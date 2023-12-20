using OpenQA.Selenium.Firefox;
using OpenQA.Selenium;
using Xunit;

namespace TestProject;
public class Test
{
	[Fact]
	public void TestFirefox()
	{
		using var driver = new FirefoxDriver();

		var userName = "Paste your mail.ru user name";
		var login = "Paste your mail.ru login here";
		var password = "Paste your mail.ru password here";

		// Устанавливаем неявный таймаут ожидания загрузки элементов
		driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
		// Делаем окно браузера во весь экран
		driver.Manage().Window.Maximize();
		// Заходим на сайт mail.ru
		driver.Navigate().GoToUrl("https://mail.ru");
		// Ищем кнопку Войти
		driver.FindElement(By.XPath("//button[@class='ph-login svelte-1ke9xx5']")).Click();
		// т.к. попап с логином находится в iframe, нужно сначала в него переключиться и запомнить handle текущего окна
		var originalWindow = driver.CurrentWindowHandle;
		var authFrame = driver.FindElement(By.XPath("//iframe[@class='ag-popup__frame__layout__iframe']"));
		driver.SwitchTo().Frame(authFrame);
		// Ищем поле логина и убеждаемся, что оно в фокусе
		var usernameElement = driver.FindElement(By.XPath("//input[@name='username']"));
		Assert.Equal(usernameElement, driver.SwitchTo().ActiveElement());
		// Очищаем его на всякий и вводим логин
		usernameElement.Clear();
		usernameElement.SendKeys(login);
		// Ищем кнопочку Ввести пароль
		driver.FindElement(By.XPath("//button[@data-test-id='next-button']")).Click();
		// Ищем поле ввода пароля и проверяем, что оно в фокусе
		var passwordElement = driver.FindElement(By.XPath("//input[@name='password']"));
		Assert.Equal(passwordElement, driver.SwitchTo().ActiveElement());
		// Очищаем его на всякий и вводим пароль
		passwordElement.Clear();
		passwordElement.SendKeys(password);
		// Ищем кнопку войти и тыкаем её
		driver.FindElement(By.XPath("//button[@data-test-id='submit-button']")).Click();
		// После авторизации переключаемся обратно на главное окно
		driver.SwitchTo().Window(originalWindow);
		// Ищем иконку с аватаркой, тыкаем её и проверяем имя пользователя
		driver.FindElement(By.XPath("//img[@class='ph-avatar-img svelte-dfhuqc']")).Click();
		Assert.Equal(userName, driver.FindElement(By.XPath("//div[@class='ph-name svelte-1popff4']")).Text);
		// Ищем кнопку Выйти и нажимаем её
		driver.FindElement(By.XPath("//div[@data-testid='whiteline-account-exit']")).Click();
		// Ищем кноку Регистрация, если она отображается, значит успешно вышли
		Assert.True(driver.FindElement(By.XPath("//a[@class='ph-project ph-project__register svelte-1ke9xx5']")).Displayed);

		driver.Quit();
	}
}
