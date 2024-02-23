# Nethereum Generators Suite

## Overview
This repository contains a suite of libraries designed to work with Ethereum, providing a comprehensive toolkit for Ethereum blockchain development in .NET.
The libraries included are redesigned versions of the well-known Nethereum libraries, with enhancements to offer more flexibility and functionality to developers.

## Libraries
The suite consists of three main libraries:

- **NethereumGenerators**: A base library for Ethereum code generation, providing core functionalities and types essential for Ethereum development.
- **NethereumGenerators.Net**: An extension of `NethereumGenerators`, tailored for .NET developers, offering seamless integration with .NET projects and additional utilities for Ethereum blockchain interaction.
- **NethereumGenerator.ExtendedConsole**: A command-line tool that leverages the capabilities of `NethereumGenerators` and `NethereumGenerators.Net`, providing an easy-to-use interface for generating Ethereum contract interaction code.
The unique feature of this console application is the generation of a `Service` class with virtual methods, enabling easy mocking of the generated `Service` class for testing purposes.

## Key Features
- **Virtual Methods in Service Class**: The `NethereumGenerator.ExtendedConsole` library generates the `Service` class with virtual methods, distinguishing it from its predecessors.
This design choice allows developers to easily mock the generated Service class, facilitating unit testing and enhancing testability.
