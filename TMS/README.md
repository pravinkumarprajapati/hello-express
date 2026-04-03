# TMS - Training Management System

This repository contains the initial module-by-module implementation for the Training Management System (TMS).

## Module 1

- Clean Architecture project skeleton
- Trainer Profile domain model
- Skill mapping model with proficiency levels
- Basic Trainer APIs (list + details)
- In-memory repository implementation for development bootstrap
- Unit tests for trainer service behavior

## Module 2

- Scheduling domain model
- Baseline auto-assignment engine
- Assignment API endpoint
- Unit tests for assignment outcomes

## Module 3

- HRMS leave pull synchronization
- Auto reassignment trigger for impacted sessions
- 60-second background sync worker
- Mock HRMS/LMS APIs

## Module 4 (Current)
- Notification templates and configuration toggles
- Notification dispatch service (email + SMS adapters)
- Notification API endpoints and audit logging

## Planned Next Modules
1. Reports and dashboard
2. SQL Server persistence migration
