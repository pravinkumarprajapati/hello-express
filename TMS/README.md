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

## Module 4

- Notification templates and configuration toggles
- Notification dispatch service (email + SMS adapters)
- Notification API endpoints and audit logging

## Module 5

- Utilization reports API
- Excel-compatible export for utilization report
- Reporting service and unit tests

## Module 6

- EF Core SQL persistence foundation
- Configurable DI switch between in-memory and SQL repositories
- LocalDB configuration for TMS API

## Module 7

- Razor Pages trainer calendar view
- Manager/Admin consolidated schedule view
- Shared web layout and styling

## Module 8

- Azure Entra SSO scaffolding in API and Web
- RBAC policy enforcement for Trainer/Manager/Admin roles
- Auth-aware Razor Pages navigation

## Module 9

- Application Insights telemetry enrichment
- Custom business telemetry publisher
- Security hardening tests for controller RBAC policies

## Module 10 (Current)
- Role validation integration tests for RBAC policies
- Security headers middleware and integration tests
- Centralized API authorization policy registration

## Planned Next Modules
1. Production deployment hardening
2. End-to-end authenticated flow tests
