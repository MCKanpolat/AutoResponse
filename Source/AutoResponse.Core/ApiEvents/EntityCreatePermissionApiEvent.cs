﻿// <copyright file="EntityCreatePermissionApiEvent.cs" company="DevDigital">
// Copyright (c) DevDigital. All rights reserved.
// </copyright>

namespace AutoResponse.Core.ApiEvents
{
    using System;

    /// <summary>
    /// Entity create permission API event.
    /// </summary>
    /// <seealso cref="AutoResponse.Core.ApiEvents.IAutoResponseApiEvent" />
    public class EntityCreatePermissionApiEvent : IAutoResponseApiEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityCreatePermissionApiEvent"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="entityId">The entity identifier.</param>
        public EntityCreatePermissionApiEvent(string code, string userId, string entityType, string entityId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            if (string.IsNullOrWhiteSpace(entityType))
            {
                throw new ArgumentNullException(nameof(entityType));
            }

            this.Code = code;
            this.UserId = userId;
            this.EntityType = entityType;
            this.EntityId = entityId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityCreatePermissionApiEvent"/> class.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="entityType">Type of the entity.</param>
        public EntityCreatePermissionApiEvent(string userId, string entityType)
            : this("AR403C", userId, entityType, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityCreatePermissionApiEvent"/> class.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="entityId">The entity identifier.</param>
        public EntityCreatePermissionApiEvent(string userId, string entityType, string entityId)
            : this("AR403C", userId, entityType, entityId)
        {
            if (string.IsNullOrWhiteSpace(entityId))
            {
                throw new ArgumentNullException(nameof(entityId));
            }
        }

        /// <summary>
        /// Gets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        public string Code { get; }

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public string UserId { get; }

        /// <summary>
        /// Gets the type of the entity.
        /// </summary>
        /// <value>
        /// The type of the entity.
        /// </value>
        public string EntityType { get; }

        /// <summary>
        /// Gets the entity identifier.
        /// </summary>
        /// <value>
        /// The entity identifier.
        /// </value>
        public string EntityId { get; }
    }
}
