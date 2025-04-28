# UBB-SE-2025-926-1

## Code Guideline

### Description

The following guideline outlines the coding standards for our merger project. It is derived from the existing ruleset, with some unnecessary or unlikely warnings/errors removed. Adhering to these guidelines will minimize the need for edits before server deployment to ensure compliance with the code ruleset.

**! Note:** This ruleset is **not definitive as of 28 April 2025**. Some rules may be removed or modified. If working before the final update, prioritize **readability, naming, and spacing** rules, as these are most likely to remain unchanged or see only minor adjustments. ** ! **

### Spacing Rules

- **SA1000–SA1005, SA1008–SA1022, SA1024–SA1026, SA1028 (Error):**
  - Ensure correct spacing for keywords, commas, semicolons, symbols, parentheses, brackets, braces, generic/attribute brackets, nullable types, member access, increment/decrement, signs, and colons.
  - Use a single space for documentation and single-line comments.
  - Avoid multiple consecutive whitespaces or trailing whitespace.
  - Do not place spaces after `new` or `stackalloc` in implicitly typed arrays.

### Readability Rules

- **SA1100, SA1102, SA1106–SA1108, SA1110–SA1115, SA1120–SA1122, SA1125, SA1127, SA1132, SA1136 (Error):**
  - Avoid unnecessary `base` prefix unless a local implementation exists.
  - Query clauses must follow prior clauses.
  - Prohibit empty statements or multiple statements on a single line.
  - Do not embed comments within block statements.
  - Ensure parentheses/brackets align with declarations/parameters.
  - Comments must contain text; use `string.Empty` for empty strings.
  - Use built-in type aliases (e.g., `int` instead of `Int32`).
  - Use shorthand for nullable types (e.g., `int?`).
  - Place generic type constraints on separate lines.
  - Avoid combined field declarations.
  - List enum values on separate lines.

### Ordering Rules

- **SA1208, SA1212 (Error):**
  - Place `System` using directives before other using directives.
  - Ensure property accessors follow a specific order (e.g., `get` before `set`).

### Naming Rules

- **SA1300, SA1302–SA1304, SA1306–SA1309, SA1311–SA1313 (Error):**
  - Elements, const fields, non-private readonly fields, accessible fields, and static readonly fields must start with uppercase letters.
  - Interfaces must start with `I`.
  - Fields, variables, and parameters must start with lowercase letters.
  - Avoid underscores or prefixes in field/variable names.

### Maintainability Rules

- **SA1119 (Warning):**
  - Avoid unnecessary parentheses in statements.
- **SA1400, SA1407–SA1408 (Error):**
  - Explicitly declare access modifiers.
  - Use parentheses to clarify precedence in arithmetic and conditional expressions.

### Layout Rules

- **SA1500–SA1512, SA1514, SA1517, SA1520 (Error):**
  - Braces for multi-line statements must not share lines and must be consistent.
  - Prohibit single-line statements or omitted braces.
  - Ensure accessors are consistently single- or multi-line.
  - Avoid blank lines after opening braces, before closing braces, or in specific contexts (e.g., chained blocks, while-do footers).
  - Prohibit multiple consecutive blank lines or blank lines at the file start.
  - Element documentation headers must be preceded by a blank line.
