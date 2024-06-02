// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;

contract StressLevel {
    uint256 public stressLevel;

    function setStressLevel(uint256 _stressLevel) external {
        stressLevel = _stressLevel;
    }
}
