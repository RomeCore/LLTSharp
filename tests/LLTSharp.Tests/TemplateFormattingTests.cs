using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.AI;

namespace LLTSharp.Tests
{
	/// <summary>
	/// The tests that verify the formatting of templates, including newlines and indentation.
	/// </summary>
	public class TemplateFormattingTests
	{
		[Fact]
		public void IfElseTemplateFormatting()
		{
			var parser = new LLTParser();

			var templateStr =
			"""
			@template if_else_format
			{
				Greetings, @name!
				@if age > 18
				{
					You are an adult.
				}
				else
				{
					You are too young!
				}

				Have a nice day.
			}
			""";

			var template = parser.Parse(templateStr).First();

			var adult = new { name = "Andrew", age = 20 };
			var young = new { name = "Alice", age = 15 };

			var renderedAdult = template.Render(adult).ToString();
			var renderedYoung = template.Render(young).ToString();

			var expectedAdult =
			"""
			Greetings, Andrew!
			You are an adult.

			Have a nice day.
			""";

			var expectedYoung =
			"""
			Greetings, Alice!
			You are too young!

			Have a nice day.
			""";

			Assert.Equal(expectedAdult, renderedAdult);
			Assert.Equal(expectedYoung, renderedYoung);
		}

		[Fact]
		public void IfElseWithMultilineFormatting()
		{
			var parser = new LLTParser();

			var templateStr =
			"""
			@template if_else_format
			{
				Greetings, @name!
				@if code_type == 'csharp'
				{
					`````
					class Program
					{
						public static void Main(string[] args)
						{
							Console.WriteLine("Hello, world, @user!");
						}
					}
					`````
				}
				else if code_type == 'python'
				{

					`````
					if __name__ == "main":
						print("Hello, world, @user!")
					`````
				}

				Have a nice day.
			}
			""";

			var template = parser.Parse(templateStr).First();

			var dataCsharp = new { name = "Andrew", code_type = "csharp" };
			var dataPython = new { name = "Alice", code_type = "python" };

			var renderedCsharp = template.Render(dataCsharp).ToString();
			var renderedPython = template.Render(dataPython).ToString();

			var expectedCsharp =
			"""
			Greetings, Andrew!
			class Program
			{
				public static void Main(string[] args)
				{
					Console.WriteLine("Hello, world, @user!");
				}
			}

			Have a nice day.
			""";

			var expectedPython =
			"""
			Greetings, Alice!

			if __name__ == "main":
				print("Hello, world, @user!")

			Have a nice day.
			""";

			Assert.Equal(expectedCsharp, renderedCsharp);
			Assert.Equal(expectedPython, renderedPython);
		}

		[Fact]
		public void ForeachTemplateFormatting()
		{
			var parser = new LLTParser();

			var templateStr =
			"""
			@template foreach_format
			{
				Grocery list:
			
				@foreach item in ctx
				{
					- @item.name: @item.quantity
				}

				End of list.
			}
			""";

			var template = parser.Parse(templateStr).First();

			var groceries = new[] {
				new { name = "Apples", quantity = 3 },
				new { name = "Bananas", quantity = 5 },
				new { name = "Oranges", quantity = 2 }
			};

			var rendered = template.Render(groceries).ToString();

			var expected =
			"""
			Grocery list:

			- Apples: 3
			- Bananas: 5
			- Oranges: 2

			End of list.
			""";

			Assert.Equal(expected, rendered);
		}

		[Fact]
		public void ForeachInlineListTemplateFormatting()
		{
			var parser = new LLTParser();

			var templateStr =
			"""
			@template foreach_format
			{
				Grocery list:
			
				@foreach item in [
					{ name: 'Fish', quantity: 5 },
					{ name: 'Meat', quantity: 10 },
					{ name: 'Eggs', quantity: 0 } @/ Why eggs is zero???
					]
				{
					@if item.quantity > 0
					{
						- @item.name: @item.quantity
					}
				}

				End of list.
			}
			""";

			var template = parser.Parse(templateStr).First();

			var rendered = template.Render().ToString();

			var expected =
			"""
			Grocery list:

			- Fish: 5
			- Meat: 10

			End of list.
			""";

			Assert.Equal(expected, rendered);
		}

		[Fact]
		public void ComplexTemplateScenarioFormatting()
		{
			var parser = new LLTParser();

			var templateStr =
			"""
			@template complex_demo
			{
				@let attempt = 1
				Processing datasets:

				@foreach dataset in [
					{ name: 'set-A', items: [1, 3, 5] },
					{ name: 'set-B', items: [2, 4, 0] },
					{ name: 'set-C', items: [] }
				]
				{
					@if attempt == 1 {
						---
					}
					@let sum = 0
					Dataset: @dataset.name
					@foreach value in dataset.items
					{
						@if value > 0
						{
							Value: @value
							@sum = (sum + value)
						}
						else
						{
							Skipping zero.
						}
					}
					@if sum > 0
					{
						Sum: @sum
						@let retry = 2
						@while retry > 0
						{
							Retry iteration @retry for @dataset.name
							@retry = (retry - 1)
						}
					}
					else
					{
						No positive values!
					}
					@attempt = (attempt + 1)
					---
				}

				All done. Total attempts: @attempt
			}
			""";

			var template = parser.Parse(templateStr).First();

			var rendered = template.Render(new { }).ToString();

			var expected =
			"""
			Processing datasets:

			---
			Dataset: set-A
			Value: 1
			Value: 3
			Value: 5
			Sum: 9
			Retry iteration 2 for set-A
			Retry iteration 1 for set-A
			---
			Dataset: set-B
			Value: 2
			Value: 4
			Skipping zero.
			Sum: 6
			Retry iteration 2 for set-B
			Retry iteration 1 for set-B
			---
			Dataset: set-C
			No positive values!
			---

			All done. Total attempts: 4
			""";

			Assert.Equal(expected, rendered);
		}

		[Fact]
		public void WhileTemplateFormatting()
		{
			var parser = new LLTParser();

			var templateStr =
			"""
			@template while_format
			{
				Counter demo:

				@let counter = 3
				@while counter > 0
				{
					Current counter: @counter
					@counter = (counter - 1)
				}

				Done.
			}
			""";

			var template = parser.Parse(templateStr).First();

			var rendered = template.Render(new { }).ToString();

			var expected =
			"""
			Counter demo:

			Current counter: 3
			Current counter: 2
			Current counter: 1

			Done.
			""";

			Assert.Equal(expected, rendered);
		}


		[Fact]
		public void NestedIfFormatting()
		{
			var parser = new LLTParser();

			var templateStr =
			"""
			@template nested_if
			{
				Greetings, @name!
				@if age > 18
				{
					@if is_admin
					{
						You are an adult admin.
					}
					else
					{
						You are an adult user.
					}
				}
				else
				{
					You are too young!
				}
			}
			""";

			var template = parser.Parse(templateStr).First();

			var admin = new { name = "Andrew", age = 25, is_admin = true };
			var user = new { name = "Alice", age = 30, is_admin = false };
			var young = new { name = "Bob", age = 15, is_admin = false };

			var renderedAdmin = template.Render(admin).ToString();
			var renderedUser = template.Render(user).ToString();
			var renderedYoung = template.Render(young).ToString();

			var expectedAdmin =
			"""
			Greetings, Andrew!
			You are an adult admin.
			""";

			var expectedUser =
			"""
			Greetings, Alice!
			You are an adult user.
			""";

			var expectedYoung =
			"""
			Greetings, Bob!
			You are too young!
			""";

			Assert.Equal(expectedAdmin, renderedAdmin);
			Assert.Equal(expectedUser, renderedUser);
			Assert.Equal(expectedYoung, renderedYoung);
		}

		[Fact]
		public void NestedTemplateRendering()
		{
			var parser = new LLTParser();

			var templateStr =
			"""
			@template nested_host
			{
				Here is groceries list:
				@render 'nested_template'
			}

			@template nested_template
			{
				Here is nested list:
				@foreach item in ctx
				{
					Item: @item
				}
			}
			""";

			var template = parser.Parse(templateStr).First();

			var groceries = new[] {
				"Apples",
				"Bananas",
				"Oranges"
			};

			var rendered = template.Render(groceries).ToString();

			var expected =
			"""
			Here is groceries list:
			Here is nested list:
			Item: Apples
			Item: Bananas
			Item: Oranges
			""";

			Assert.Equal(expected, rendered);
		}

		[Fact]
		public void ForeachVariableShadowing()
		{
			var template = new LLTParser().Parse(
			"""
			@template t {
				@foreach item in items {
					Outer: @item
					@let item = 'shadowed'
					Inner: @item
				}
			}
			""").First();

			var rendered = template.Render(new { items = new[] { "A", "B" } }).ToString();

			var expected =
			"""
			Outer: A
			Inner: shadowed
			Outer: B
			Inner: shadowed
			""";

			Assert.Equal(expected, rendered);
		}

		[Fact]
		public void MessagesTemplateFormatting()
		{
			var parser = new LLTParser();

			var templateStr =
			"""
			@messages template test_messages
			{
				@system message
				{
					You are a helpful assistant.
					
					Here is your instructions:
					@let a = 1
					@foreach instruction in instructions
					{
						Instruction @a: @instruction
						@a = (a + 1)
					}
				}

				@foreach name in names
				{
					@message
					{
						@role 'user'
						Hello, i am @name!
					}
				}
			}
			""";

			var template = parser.Parse(templateStr).First();

			var context = new
			{
				names = new[] { "Alex", "Rob", "John" },
				instructions = new[] { "Do this", "Do that" }
			};

			var messages = (IEnumerable<ChatMessage>)template.Render(context);

			var system = messages.ElementAt(0);
			var user1 = messages.ElementAt(1);
			var user2 = messages.ElementAt(2);
			var user3 = messages.ElementAt(3);

			var expectedSystemContent =
			"""
			You are a helpful assistant.

			Here is your instructions:
			Instruction 1: Do this
			Instruction 2: Do that
			""";

			Assert.Equal(ChatRole.System, system.Role);
			Assert.Equal(expectedSystemContent, system.Text);
			Assert.Equal(ChatRole.User, user1.Role);
			Assert.Equal("Hello, i am Alex!", user1.Text);
			Assert.Equal("Hello, i am Rob!", user2.Text);
			Assert.Equal("Hello, i am John!", user3.Text);
		}
	}
}