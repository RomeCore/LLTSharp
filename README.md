# LLTSharp

[![Build](https://github.com/RomeCore/LLTSharp/actions/workflows/build.yml/badge.svg)](https://github.com/RomeCore/LLTSharp/actions/workflows/build.yml)
[![Tests](https://github.com/RomeCore/LLTSharp/actions/workflows/tests.yml/badge.svg)](https://github.com/RomeCore/LLTSharp/actions/workflows/tests.yml)

A flexible and expressive **template engine for Large Language Model (LLM) prompts and structured message generation** in C#.  
LLT is designed to make prompt engineering and content generation as powerful and maintainable as regular C# code.

## ✨ Key Features

- **Razor-inspired DSL** with `@if`, `@foreach`, expressions, metadata, and inline variables  
- **Message-oriented syntax** for LLM role structures (system, user, assistant)  
- **Powerful metadata filtering** — select the best template by language, model, or custom qualifiers  
- **Composable templates** — reuse and render nested templates inside others  
- **Deterministic formatting** — preserves whitespace and indentation, ensuring predictable output  
- **Expression evaluator** — supports arithmetic, logic, method access, array indexing, and ternary operators  
- **Library-driven workflow** — import templates from assemblies, files, or strings  

## 📦 Installation

Install the package from NuGet:

```
dotnet add package LLTSharp
```

Or via the Package Manager Console:

```
Install-Package LLTSharp
```

## 💡 Example

```csharp
var parser = new LLTParser();

var templateStr = """
@template GreetingTemplate
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

Console.WriteLine(template.Render(adult));
Console.WriteLine(template.Render(young));
```

**Output:**
```
Greetings, Andrew!
You are an adult.

Have a nice day.

Greetings, Alice!
You are too young!

Have a nice day.
```

## 🧩 Template Library Usage

Multiple templates can be stored, versioned, and retrieved by language or model ID:

```csharp
var lib = new TemplateLibrary();

lib.ImportFromString("""
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
    @metadata { lang: 'es' }
    Hola!
}
""");

var template = lib.Retrieve("greeting", new LanguageMetadata("en"), new TargetModelMetadata("gpt-4"));
Console.WriteLine(template.Render()); // Hello GPT-4!
```

Templates are resolved by **metadata specificity**, similar to CSS selector priority.

## 💬 Message Templates for LLM Chats

LLT supports special `@messages` syntax for structured chat prompts:

```llt
@messages template ChatBot
{
    @metadata { language: 'en', version: 1 }

    @system message {
        You are a helpful assistant.

        Here is your instructions:
        @foreach instruction in instructions {
            Instruction: @instruction
        }
    }

    @foreach name in names {
        @message {
            @role 'user'
            Hello, I am @name!
        }
    }
}
```

**Rendered result:** a sequence of chat messages with proper roles (`system`, `user`, `assistant`).

## 🧠 Supported Syntax

- Inline expressions: `@(a + b * c)`
- Conditionals:  
  ```llt
  @if cond {
      ...
  } else if other {
      ...
  } else {
      ...
  }
  ```
- Loops:  
  ```llt
  @foreach item in items {
      @item
  }
  ```
- Variables:  
  ```llt
  @let x = 5
  @x = 10
  @x
  ```
- Nested or external templates:  
  ```llt
  @render 'other_template'
  ```
- Comments:  
  ```llt
  @// line comment
  @*
    block comment
  *@
  ```

## 🧾 Formatting & Scoping Rules

- Whitespace and indentation in source templates are **fully preserved**
- `@let` variables are **lexically scoped**
- Loop variables do **not leak outside** their block
- Nested `@if` and `@foreach` blocks behave predictably, matching C#‑like logical semantics

## 🏗️ Architecture Overview

- **LLTParser** – parses source text into ASTs (`TemplateNode`, `TemplateExpressionNode`, etc.)
- **Template** – runtime object capable of rendering with dynamic context
- **TemplateLibrary** – registry and loader for collections of templates with filtering
- **Metadata System** – extensible mechanism for attaching attributes such as language, model, or custom version
- **ChatMessage Model** – unified representation for structured message templates (`system`, `user`, `assistant`)

## 🔖 License

MIT License © 2025
