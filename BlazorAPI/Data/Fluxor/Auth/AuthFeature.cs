﻿using Fluxor;

namespace BlazorAPI.Data.Fluxor.Auth
{
    [FeatureState]
    public class AuthFeature : Feature<AuthState>
    {
        public override string GetName() => "Auth";
        protected override AuthState GetInitialState() => AuthState.Empty;
    }
}
