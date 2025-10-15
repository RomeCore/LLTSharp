using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LLTSharp.Metadata;

namespace LLTSharp.Tests
{
	public class TemplateLibraryTests
	{
		[Fact]
		public void TemplateLibraryEmbedResourceImport()
		{
			var lib = new TemplateLibrary();

			lib.ImportFromAssembly(typeof(TemplateLibraryTests).Assembly);

			var template = lib.Retrieve("sample");
			var rendered = template.Render();

			Assert.Equal("Hello, World! This is a template file from embedded resources!", rendered);
		}

		[Fact]
		public void TemplateLibraryFullImport()
		{
			var lib = new TemplateLibrary();

			lib.ImportFromString(
			"""
			@template sample_template_another
			{
				@metadata
				{
					lang: 'en_US',
					model: 'gpt-3.5-turbo'
				}
				This is another template for GPT-3.5-Turbo model.
			}
			
			@template sample_template
			{
				@metadata
				{
					lang: 'en_US',
					model: 'gpt-3.5-turbo'
				}
				This is template for GPT-3.5-Turbo model.
			}

			@template sample_template
			{
				@metadata
				{
					lang: 'en_US',
					model: 'gpt-4'
				}
				This is template for GPT-4 model.
			}
			""");

			var template = lib.Retrieve("sample_template", new LanguageMetadata("en_US"), new TargetModelMetadata("gpt-3.5-turbo"));
			var rendered = template.Render();
			Assert.Equal("This is template for GPT-3.5-Turbo model.", rendered);

			template = lib.Retrieve("sample_template", new LanguageMetadata("en_US"), new TargetModelMetadata("gpt-4"));
			rendered = template.Render();
			Assert.Equal("This is template for GPT-4 model.", rendered);

			Assert.Throws<KeyNotFoundException>(() => lib.Retrieve("sample_template", new LanguageMetadata("fr_FR")));
		}

		[Fact]
		public void TemplatePriorityByMetadataSpecificity()
		{
			var lib = new TemplateLibrary();

			lib.ImportFromString(
			"""
			@template greeting
			{
				Generic greeting
			}

			@template greeting
			{
				@metadata { lang: 'en' }
				Hello!
			}

			@template greeting
			{
				@metadata { lang: 'en', model: 'gpt-4' }
				Hello GPT-4!
			}

			@template greeting
			{
				@metadata { lang: 'ru' }
				Привет!
			}
			""");

			// Most specific template
			var specific = lib.Retrieve("greeting",
				new LanguageMetadata("en"),
				new TargetModelMetadata("gpt-4"));
			Assert.Equal("Hello GPT-4!", specific.Render().ToString());

			// Less specific template, but still more specific than the general one.
			var lessSpecific = lib.Retrieve("greeting", new LanguageMetadata("en"));
			Assert.Equal("Hello!", lessSpecific.Render().ToString());

			// Most general template.
			var generic = lib.Retrieve("greeting");
			Assert.Equal("Generic greeting", generic.Render().ToString());
		}
	}
}