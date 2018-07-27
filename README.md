# DynamicsCrm-AutoNumbering
### Version: 1.1
---

A CRM solution that gives a lot of flexibility in creating any pattern required for auto-numbering.

### Features

  + String, date, parameter, and attribute patterns
  + Run numbering on a condition
  + Random strings, with number-to-letter ratios, and "start with a letter" flag
  + Optional numbering sequence, with padding
  + Reset interval -- periodic or one-time reset -- for numbering sequence
  + Locking when busy to avoid duplicate indexes
  + Option to use plugin instead of workflow step, which allows the generation of numbering for entities that lock after the operation
  + Option to validate unique generated string
  + Option to generate without updating a record (return the generated string only)
  + Support for plugin step inline configuration

### Dependencies

  + Common.cs
    + Can be found in the DynamicsCrm-Libraries repository
    + Should be added to the root of the solution
  + CRM Logger solution

---
**Copyright &copy; by Ahmed el-Sawalhy ([YagaSoft](http://yagasoft.com))** -- _GPL v3 Licence_